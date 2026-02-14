import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Login from '../Pages/Login';
import Register from '../Pages/Register';
import Categorias from '../Pages/Categorias';
import DashBoard from '../Pages/Dashboard';
import SideBar from '../Pages/SideBar';
import Lancamentos from '../Pages/Lancamentos';
import Contas from '../Pages/Contas';
import Users from '../Pages/Users';

const ProtectedRoute = ({ children }) => {
    const token = localStorage.getItem('AuthToken');
    return token ? children : <Navigate to="/login" replace />;
};
function AppRoutes(){
    return(
        <Routes>
            <Route path="/" element={<ProtectedRoute><SideBar/> </ProtectedRoute>} />
            <Route path="/dashboard" element={<ProtectedRoute><DashBoard/> </ProtectedRoute>} />
            <Route path="/categoria" element={<ProtectedRoute><Categorias/> </ProtectedRoute>} />
            <Route path="/lancamentos" element={<ProtectedRoute><Lancamentos/> </ProtectedRoute>} />
            <Route path="/contas" element={<ProtectedRoute><Contas/> </ProtectedRoute>} />
            <Route path='/users' element={<ProtectedRoute><Users/></ProtectedRoute>} />
            <Route path="/login" element={<Login/>} />
            <Route path="/register" element={<Register/>} />
        </Routes>
    )
}
export default AppRoutes;