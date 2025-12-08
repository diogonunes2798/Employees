function ErrorModal({ message, onClose }) {
  if (!message) return null

  return (
    <div className="modal-backdrop error-layer" role="alertdialog" aria-live="assertive">
      <div className="modal">
        <div className="modal-header error">
          <strong>Error</strong>
          <button type="button" className="close" onClick={onClose} aria-label="Close">
            X
          </button>
        </div>
        <p className="modal-body">{message}</p>
      </div>
    </div>
  )
}

export default ErrorModal
