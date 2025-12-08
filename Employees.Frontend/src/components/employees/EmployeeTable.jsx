import EmployeeRow from './EmployeeRow'

function EmployeeTable({ employees, sort, onSort, onEdit, onDelete, deletingId }) {
  if (!employees.length) {
    return <p className="muted">No employees yet.</p>
  }

  return (
    <div className="table-wrap">
      <table className="employee-table">
        <thead>
          <tr>
            <th>#</th>
            <th>
              <button type="button" className="sortable" onClick={() => onSort('name')}>
                Name
                {sort.field === 'name' && (
                  <span className="sort-indicator">{sort.direction === 'asc' ? '^' : 'v'}</span>
                )}
              </button>
            </th>
            <th>
              <button type="button" className="sortable" onClick={() => onSort('dateOfBirth')}>
                Date of birth
                {sort.field === 'dateOfBirth' && (
                  <span className="sort-indicator">{sort.direction === 'asc' ? '^' : 'v'}</span>
                )}
              </button>
            </th>
            <th>
              <button type="button" className="sortable" onClick={() => onSort('yearsOfExperience')}>
                Experience (years)
                {sort.field === 'yearsOfExperience' && (
                  <span className="sort-indicator">{sort.direction === 'asc' ? '^' : 'v'}</span>
                )}
              </button>
            </th>
            <th>Technologies</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {employees.map((employee, index) => (
            <EmployeeRow
              key={employee.id}
              employee={employee}
              index={index}
              onEdit={onEdit}
              onDelete={onDelete}
              deletingId={deletingId}
            />
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default EmployeeTable
