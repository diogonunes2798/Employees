function DeleteConfirmModal({ name, loading, onCancel, onConfirm }) {
  return (
    <div className="modal-backdrop" role="dialog" aria-modal="true">
      <div className="modal">
        <div className="modal-header">
          <strong>Confirm</strong>
          <button type="button" className="close" onClick={onCancel} aria-label="Close">
            X
          </button>
        </div>
        <p className="modal-body">Are you sure you want to delete {name || 'this employee'}?</p>
        <div className="modal-actions">
          <button type="button" className="ghost small" onClick={onCancel}>
            Cancel
          </button>
          <button type="button" className="danger small" onClick={onConfirm} disabled={loading}>
            {loading ? 'Deleting...' : 'Delete'}
          </button>
        </div>
      </div>
    </div>
  )
}

export default DeleteConfirmModal
