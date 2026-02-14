import { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css"; 
import "bootstrap-icons/font/bootstrap-icons.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";
import { useNavigate } from "react-router-dom";
import { GetUserInformation } from "../Services/Security/Api";
import Users from "../Pages/Users";
import DashBoard from "../Pages/Dashboard";
import Lancamentos from "../Pages/Lancamentos";
import Categorias from "../Pages/Categorias";
import Contas from "../Pages/Contas";

function Sidebar() {
    const navigate = useNavigate();
    const [content, setContent] = useState();
    const [UserName, setUserName ] = useState("");
    const handleLogout = async () => {
        confirm("Deseja realmente sair?");
        if(confirm){
            localStorage.removeItem("AuthToken");
            localStorage.removeItem("UserId");
            navigate("/login");
        }
    }
    useEffect(() => {
        const fetchUserInfo = async () => {
            const token = localStorage.getItem("AuthToken");
            if (!token) {
                navigate("/login");
                return;
            }
            const response = await GetUserInformation(token);
            if(response.statusCode === 401){
                localStorage.removeItem("AuthToken");
                navigate("/login");
            }
            else 
            {
                setUserName(response.nome);
            }
        };
        fetchUserInfo();
        const handleSetUserNameLogged = () => {
            fetchUserInfo();
        };
        window.addEventListener("setUserNameLogged", handleSetUserNameLogged);
    }, [navigate]);

    const handleNavigation = (path) => {
        switch (path) {
            case "/dashboard":
                setContent(<DashBoard/>);
                break;
            case "/lancamentos":
                setContent(<Lancamentos/>);
                break;
            case "/users":
                setContent(<Users/>);
                break;  
            case "/categoria":
                setContent(<Categorias/>);    
                break;
            case "/contas":
                setContent(<Contas/>);    
                break;
        }
    };
    return (
        <div className="container-fluid">
            <div className="row flex-nowrap">
                <div className="col-auto col-md-3 col-xl-2 px-sm-2 px-0 bg-dark">
                    <div className="d-flex flex-column align-items-center align-items-sm-start px-3 pt-2 text-white min-vh-100">
                        <a href="#" className="d-flex align-items-center pb-3 mb-md-0 me-md-auto text-white text-decoration-none">
                            <span className="fs-5 d-none d-sm-inline">Control Finance</span>
                        </a>    
                        <ul className="nav nav-pills flex-column mb-sm-auto mb-0 align-items-center align-items-sm-center" id="menu">
                            <li className="nav-item">
                                <a href="#" className="nav-link align-middle px-0" onClick={() => handleNavigation("/dashboard")}>
                                    <i className="fs-4 bi-house"></i> <span className="ms-1 d-none d-sm-inline">DashBoard</span>
                                </a>
                                <a href="#" className="nav-link px-0 align-middle" onClick={() => handleNavigation("/lancamentos")}>
                                    <i className="fs-4 bi-table"></i> <span className="ms-1 d-none d-sm-inline">Lancamentos</span>
                                </a>
                                <a href="#" className="nav-link px-0 align-middle" onClick={() => handleNavigation("/categoria")}>
                                    <i className="fs-4 bi-speedometer2"></i> <span className="ms-1 d-none d-sm-inline">Categorias</span> 
                                </a>
                                <a href="#" className="nav-link px-0 align-middle" onClick={() => handleNavigation("/contas")}>
                                    <i className="fs-4 bi-speedometer2"></i> <span className="ms-1 d-none d-sm-inline">Contas</span> 
                                </a>
                                <a href="#" className="nav-link px-0 align-middle" onClick={() => handleNavigation("/users")}>
                                    <i className="fs-4 bi-speedometer2"></i> <span className="ms-1 d-none d-sm-inline">Usuários</span> 
                                </a>
                            </li>                                
                        </ul>
                        <hr />
                        <div className="dropdown pb-4">
                            <a href="#" className="d-flex align-items-center text-white text-decoration-none dropdown-toggle" id="dropdownUser1" data-bs-toggle="dropdown" aria-expanded="false">
                                <span className="d-none d-sm-inline mx-1">{UserName}</span>
                            </a>
                            <ul className="dropdown-menu dropdown-menu-dark text-small shadow">
                                <li><a className="dropdown-item" onClick={handleLogout}>Sair</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div className="col py-3">
                    {content}
                </div>
            </div>
        </div>
    );
}

export default Sidebar;