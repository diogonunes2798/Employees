function Sidebar() {
  return (
    <aside className="sidebar">
      <div className="brand">
        <div className="brand-mark">E</div>
        <div>
          <p className="brand-title">Employees</p>
          <p className="brand-subtitle">Frontend</p>
        </div>
      </div>
      <nav className="nav">
        <p className="nav-label">Menu</p>
        <button className="nav-item active">Employees</button>
      </nav>
    </aside>
  )
}

export default Sidebar
