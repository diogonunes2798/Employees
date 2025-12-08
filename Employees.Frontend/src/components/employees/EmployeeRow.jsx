function EmployeeRow({ employee, index, onEdit, onDelete, deletingId }) {
  return (
    <tr>
      <td>{index + 1}</td>
      <td className="name">
        <button type="button" className="link-button" onClick={() => onEdit(employee.id)} title="Edit employee">
          {employee.name}
        </button>
      </td>
      <td>{employee.dateOfBirth ? new Date(employee.dateOfBirth).toLocaleDateString() : '-'}</td>
      <td>{employee.yearsOfExperience}</td>
      <td>
        <div className="tags">
          {employee.technologies?.length ? (
            employee.technologies.map((tech) => (
              <span key={tech} className="tag">
                {tech}
              </span>
            ))
          ) : (
            <span className="tag muted">No technologies</span>
          )}
        </div>
      </td>
      <td>
        <button
          type="button"
          className="danger icon-button"
          onClick={() => onDelete(employee)}
          aria-label="Delete employee"
          title="Delete"
          disabled={deletingId === employee.id}
        >
          {deletingId === employee.id ? (
            '...'
          ) : (
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="16"
              height="16"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
              strokeLinecap="round"
              strokeLinejoin="round"
              aria-hidden="true"
            >
              <path d="M3 6h18" />
              <path d="M8 6v14a2 2 0 0 0 2 2h4a2 2 0 0 0 2-2V6" />
              <path d="M10 11v6" />
              <path d="M14 11v6" />
              <path d="M9 6V4a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v2" />
            </svg>
          )}
        </button>
      </td>
    </tr>
  )
}

export default EmployeeRow
