import { useEffect } from "react";
import {useNavigate} from "react-router-dom";
import AppRoutes from "./Services/Routes";

const App = () => {
    const navigate = useNavigate();
    useEffect(() => {
        const CheckToken = async () => {
        try
        {
            if(window.location.pathname === '/register')return;
            const token = localStorage.getItem('AuthToken');
            if(!token){
                navigate('/login'); 
                return;
            } 
            else{
                navigate('/');
            } 
        }
        catch(error){
            localStorage.removeItem('AuthToken');
            console.error("Erro ao vlidar token:", error);    
            navigate('/login');
        }
    };
    CheckToken();
    }, [navigate]);
    return <AppRoutes />; 
}
export default App;