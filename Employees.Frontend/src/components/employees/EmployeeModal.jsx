function EmployeeModal({
  open,
  loading,
  saving,
  form,
  onChange,
  onClose,
  onSubmit,
  editingId,
}) {
  if (!open) return null

  return (
    <div className="modal-backdrop">
      <div className="modal">
        <div className="modal-header">
          <strong>{editingId ? 'Edit employee' : 'New employee'}</strong>
          <button
            type="button"
            className="close"
            onClick={() => {
              onClose()
            }}
            aria-label="Close"
          >
            X
          </button>
        </div>

        {loading ? (
          <p className="modal-body">Loading employee...</p>
        ) : (
          <form className="form" onSubmit={onSubmit}>
            <label>
              Name
              <input
                name="name"
                value={form.name}
                onChange={onChange}
                placeholder="e.g. Maria Silva"
                required
              />
            </label>

            <label>
              Date of birth
              <input type="date" name="dateOfBirth" value={form.dateOfBirth} onChange={onChange} required />
            </label>

            <label>
              Years of experience
              <input
                type="number"
                min="0"
                name="yearsOfExperience"
                value={form.yearsOfExperience}
                onChange={onChange}
                placeholder="e.g. 3"
                required
              />
            </label>

            <label>
              Technologies (comma separated)
              <input
                name="technologies"
                value={form.technologies}
                onChange={onChange}
                placeholder="e.g. React, .NET, SQL"
              />
            </label>

            <div className="modal-actions">
              <button type="submit" disabled={saving}>
                {saving ? 'Saving...' : editingId ? 'Update employee' : 'Create employee'}
              </button>
            </div>
          </form>
        )}
      </div>
    </div>
  )
}

export default EmployeeModal
