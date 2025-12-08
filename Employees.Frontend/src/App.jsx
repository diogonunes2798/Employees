import { useEffect, useState } from 'react'
import { createEmployee, deleteEmployee, fetchEmployees, getEmployee, updateEmployee } from './api'
import Sidebar from './components/layout/Sidebar'
import PageHeader from './components/layout/PageHeader'
import ErrorModal from './components/shared/ErrorModal'
import SearchBar from './components/shared/SearchBar'
import DeleteConfirmModal from './components/shared/DeleteConfirmModal'
import EmployeeModal from './components/employees/EmployeeModal'
import EmployeeTable from './components/employees/EmployeeTable'
import './App.css'

const emptyForm = {
  name: '',
  dateOfBirth: '',
  yearsOfExperience: '',
  technologies: '',
}

function App() {
  const [employees, setEmployees] = useState([])
  const [form, setForm] = useState(emptyForm)
  const [searchTerm, setSearchTerm] = useState('')
  const [loading, setLoading] = useState(false)
  const [creating, setCreating] = useState(false)
  const [deletingId, setDeletingId] = useState(null)
  const [confirmDelete, setConfirmDelete] = useState(null)
  const [showCreateModal, setShowCreateModal] = useState(false)
  const [prefillLoading, setPrefillLoading] = useState(false)
  const [editingId, setEditingId] = useState(null)
  const [sort, setSort] = useState({ field: '', direction: 'asc' })
  const [error, setError] = useState('')
  const [showError, setShowError] = useState(false)

  useEffect(() => {
    loadEmployees()
  }, [])

  const loadEmployees = async () => {
    setLoading(true)
    setError('')
    setShowError(false)

    try {
      const data = await fetchEmployees()
      setEmployees(data)
    } catch (err) {
      setError(err.message || 'Failed to load employees.')
      setShowError(true)
    } finally {
      setLoading(false)
    }
  }

  const handleChange = (event) => {
    const { name, value } = event.target
    setForm((prev) => ({ ...prev, [name]: value }))
  }

  const handleSubmit = async (event) => {
    event.preventDefault()
    setCreating(true)
    setError('')
    setShowError(false)

    const payload = {
      name: form.name.trim(),
      dateOfBirth: form.dateOfBirth ? new Date(form.dateOfBirth).toISOString() : '',
      yearsOfExperience: Number(form.yearsOfExperience) || 0,
      technologies: form.technologies
        .split(',')
        .map((tech) => tech.trim())
        .filter(Boolean),
    }

    try {
      if (editingId) {
        await updateEmployee(editingId, payload)
      } else {
        await createEmployee(payload)
      }
      setForm(emptyForm)
      setEditingId(null)
      setShowCreateModal(false)
      await loadEmployees()
    } catch (err) {
      setError(err.message || 'Could not save employee.')
      setShowError(true)
    } finally {
      setCreating(false)
    }
  }

  const handleDeleteConfirm = async () => {
    if (!confirmDelete?.id) return
    setDeletingId(confirmDelete.id)
    setError('')
    setShowError(false)

    try {
      await deleteEmployee(confirmDelete.id)
      setConfirmDelete(null)
      await loadEmployees()
    } catch (err) {
      setError(err.message || 'Could not delete employee.')
      setShowError(true)
    } finally {
      setDeletingId(null)
    }
  }

  const handleOpenCreate = () => {
    setEditingId(null)
    setForm(emptyForm)
    setShowCreateModal(true)
  }

  const handleOpenEdit = async (id) => {
    setShowCreateModal(true)
    setPrefillLoading(true)
    setEditingId(id)
    setError('')
    setShowError(false)

    try {
      const data = await getEmployee(id)
      setForm({
        name: data.name || '',
        dateOfBirth: data.dateOfBirth ? data.dateOfBirth.slice(0, 10) : '',
        yearsOfExperience: data.yearsOfExperience ?? '',
        technologies: Array.isArray(data.technologies) ? data.technologies.join(', ') : '',
      })
    } catch (err) {
      setShowCreateModal(false)
      setEditingId(null)
      setError(err.message || 'Could not fetch employee.')
      setShowError(true)
    } finally {
      setPrefillLoading(false)
    }
  }

  const filteredEmployees = employees.filter((employee) => {
    const term = searchTerm.trim().toLowerCase()
    if (!term) return true
    const matchesName = employee.name?.toLowerCase().includes(term)
    const matchesTech = employee.technologies?.some((tech) => tech?.toLowerCase().includes(term))
    return matchesName || matchesTech
  })

  const sortedEmployees = [...filteredEmployees].sort((a, b) => {
    if (!sort.field) return 0
    const dir = sort.direction === 'asc' ? 1 : -1

    if (sort.field === 'name') {
      return (a.name || '').localeCompare(b.name || '') * dir
    }

    if (sort.field === 'dateOfBirth') {
      const aDate = a.dateOfBirth ? new Date(a.dateOfBirth).getTime() : 0
      const bDate = b.dateOfBirth ? new Date(b.dateOfBirth).getTime() : 0
      return (aDate - bDate) * dir
    }

    if (sort.field === 'yearsOfExperience') {
      return ((a.yearsOfExperience || 0) - (b.yearsOfExperience || 0)) * dir
    }

    return 0
  })

  const handleSort = (field) => {
    setSort((prev) => {
      if (prev.field === field) {
        return { field, direction: prev.direction === 'asc' ? 'desc' : 'asc' }
      }
      return { field, direction: 'asc' }
    })
  }

  return (
    <div className="app-shell">
      <ErrorModal message={showError ? error : ''} onClose={() => setShowError(false)} />

      {confirmDelete && (
        <DeleteConfirmModal
          name={confirmDelete.name}
          loading={deletingId === confirmDelete.id}
          onCancel={() => setConfirmDelete(null)}
          onConfirm={handleDeleteConfirm}
        />
      )}

      <EmployeeModal
        open={showCreateModal}
        loading={prefillLoading}
        saving={creating}
        form={form}
        onChange={handleChange}
        onClose={() => {
          setShowCreateModal(false)
          setEditingId(null)
        }}
        onSubmit={handleSubmit}
        editingId={editingId}
      />

      <Sidebar />

      <main className="content">
        <PageHeader />

        <div className="card">
          <div className="table-toolbar">
            <div className="toolbar-left">
              <button type="button" className="primary" onClick={handleOpenCreate}>
                + Create employee
              </button>
            </div>
            <div className="toolbar-right">
              <SearchBar
                value={searchTerm}
                onChange={setSearchTerm}
                placeholder="Search by name or technology"
              />
            </div>
          </div>

          {loading ? (
            <p className="muted">Loading employees...</p>
          ) : sortedEmployees.length === 0 ? (
            <p className="muted">No employees yet.</p>
          ) : (
            <EmployeeTable
              employees={sortedEmployees}
              sort={sort}
              onSort={handleSort}
              onEdit={handleOpenEdit}
              onDelete={(emp) => setConfirmDelete({ id: emp.id, name: emp.name })}
              deletingId={deletingId}
            />
          )}
        </div>
      </main>
    </div>
  )
}

export default App
