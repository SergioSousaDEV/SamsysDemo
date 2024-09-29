import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './App.css'
import EditClient from './pages/client/editClient'
import Home from './pages/home'
import CreateClient from './pages/client/createClient'
import GetAllPaginatedClient from './pages/client/getAllPaginatedClient'
import ViewClient from './pages/client/viewClient'


function App() {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route
            path={"/"}
            element={<Home />}
          />

          <Route
            path="/clients"
            element={<GetAllPaginatedClient />}
          />

          <Route
            path="/client/create"
            element={<CreateClient/>}
          />

          <Route
            path="/client/edit/:id"
            element={<EditClient />}
          />

          <Route
            path="/client/:id"
            element={<ViewClient/>}
          />
        </Routes>
      </BrowserRouter>
    </>
  )
}

export default App
