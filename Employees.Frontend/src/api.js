const API_BASE_URL = (import.meta.env.VITE_API_BASE_URL || 'https://localhost:5001/api/v1').replace(/\/$/, '')
const API_KEY = import.meta.env.VITE_API_KEY || ''

const defaultHeaders = API_KEY ? { 'X-API-KEY': API_KEY } : {}

const normalizeEmployee = (employee) => ({
  id: employee.id ?? employee.Id,
  name: employee.name ?? employee.Name,
  dateOfBirth: employee.dateOfBirth ?? employee.DateOfBirth,
  yearsOfExperience: employee.yearsOfExperience ?? employee.YearsOfExperience,
  technologies: employee.technologies ?? employee.Technologies ?? [],
})

const parseEmployees = (data) => {
  const items = data?.items ?? data?.Items ?? []
  return Array.isArray(items) ? items.map(normalizeEmployee) : []
}

export async function fetchEmployees() {
  const response = await fetch(`${API_BASE_URL}/employees`, {
    headers: defaultHeaders,
  })

  if (!response.ok) {
    throw new Error(
      await extractMessage(response, 'Unable to fetch employees. Check API and API key.')
    )
  }

  const data = await response.json()
  return parseEmployees(data)
}

export async function getEmployee(id) {
  const response = await fetch(`${API_BASE_URL}/employees/${id}`, {
    headers: defaultHeaders,
  })

  if (!response.ok) {
    throw new Error(await extractMessage(response, 'Unable to fetch employee.'))
  }

  const data = await response.json()
  return normalizeEmployee(data)
}

export async function createEmployee(employee) {
  const response = await fetch(`${API_BASE_URL}/employees`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      ...defaultHeaders,
    },
    body: JSON.stringify(employee),
  })

  if (!response.ok) {
    throw new Error(await extractMessage(response, 'Failed to create employee.'))
  }

  return response.json()
}

export async function updateEmployee(id, employee) {
  const response = await fetch(`${API_BASE_URL}/employees/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      ...defaultHeaders,
    },
    body: JSON.stringify(employee),
  })

  if (!response.ok) {
    throw new Error(await extractMessage(response, 'Failed to update employee.'))
  }

  return response.json()
}

export async function deleteEmployee(id) {
  const response = await fetch(`${API_BASE_URL}/employees/${id}`, {
    method: 'DELETE',
    headers: defaultHeaders,
  })

  if (!response.ok) {
    throw new Error(await extractMessage(response, 'Failed to delete employee.'))
  }
}

async function extractMessage(response, fallback) {
  try {
    const body = await response.clone().json()
    if (Array.isArray(body?.erros) && body.erros.length) {
      const details = body.erros
        .map((e) => {
          if (e?.propertyName && e?.message) {
            return `${e.propertyName}: ${e.message}`
          }
          return e?.message || e?.toString()
        })
        .filter(Boolean)
      if (details.length) return details.join(' | ')
    }
    if (body?.message) return body.message
    if (body?.title) return body.title
  } catch (err) {
    
  }

  try {
    const text = await response.text()
    return text || fallback
  } catch (err) {
    return fallback
  }
}
