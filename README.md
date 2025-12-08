# Employees – CRUD Web API + Front End

Aplicação completa para gestão de colaboradores (Employees) composta por:
- Employees.Api – Web API em .NET 8 com CRUD completo
- Employees.Frontend – Aplicação em React (Vite) para consumir a API

O objetivo é demonstrar uma solução simples mas completa, com API REST, persistência em SQLite, frontend leve, pesquisa, ordenação e boas práticas de clean architecture.

---
## 🚀Correr Projeto
**Backend – Employees.Api**

- dotnet restore
- dotnet build
- dotnet run --project Employees.Api
  
 Swagger disponível por defeito em:
  - https://localhost:5001/swagger

**Frontend – Employees.Frontend**

- cd .\Employees.Frontend\
- npm install
- npm run dev
  
O Vite mostra o URL (normalmente http://localhost:5173)

## 📋 Requisitos do Desafio

**Descrição:**  
Criar uma aplicação web para listar e gerir colaboradores.

**Objetivos funcionais:**

- Inserir colaboradores (Create)
- Atualizar colaboradores (Update)
- Eliminar colaboradores (Delete)  
  - Antes de eliminar deve ser efetuada uma pergunta de confirmação
- Visualizar detalhe de um colaborador (Read by Id)
- Listar colaboradores (Read all)
- Pesquisar colaboradores (por nome, tecnologia, etc.)
- (Nice-to-have) Ordenar resultados
- (Nice-to-have) Filtrar resultados

**Dados do colaborador:**

- `Id` – Identificador único
- `Nome` – Nome completo
- `Data de Nascimento`
- `Anos de Experiência`
- `Tecnologia(s)` com maior conhecimento

---

## 🏗️ Arquitetura / Visão Geral

Este projeto está dividido em:

- **API / Backend**
  - Exposta como uma Web API REST para operações sobre `Employees`
  - Endpoints para criar, ler, atualizar e eliminar colaboradores
  - Persistência em base de dados SQLite através de micro-ORM Dapper

- **Frontend**
  - Aplicação web React
  - Consome a API para:
    - Listar colaboradores
    - Pesquisar, ordenar e filtrar
    - Formulário de criação/edição
    - Diálogo de confirmação para eliminação

---

## 🔐 Autenticação: API Key
A API está protegida com API Key, enviada sempre no header:
- X-Api-Key: 'chave'

A chave encontra-se no ficheiro:
- Employees.Api/appsettings.json

---

## ↕️ Ordenação (Sort) no Endpoint GET /employees

O endpoint GET /employees suporta ordenação através do parâmetro sort.

**Sintaxe:**

 - +Name → ordena por Name ASC
 - -Name → ordena por Name DESC
 - +YearsOfExperience → ASC
 - -YearsOfExperience → DESC

**Colunas suportadas:**
- "Name"
- "YearsOfExperience"

**Exemplos:**
- GET /employees?sort=+Name
- GET /employees?sort=-YearsOfExperience
- GET /employees?sort=+Name&search=react

---

## 🧰 Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/) / ASP.NET Core Web API  
- [SQLite](https://www.sqlite.org/index.html) como base de dados local/anexada ao projeto  
- [Dapper] para acesso a dados  
- [Swagger / Swashbuckle] para documentação da API  
- React  para o frontend

---

## 🗄️ Modelo de Dados

Entidade principal: **Employee**

```csharp
public class Employee
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public int YearsOfExperience { get; set; }
    public List<string> Technologies { get; set; } = new();
}
