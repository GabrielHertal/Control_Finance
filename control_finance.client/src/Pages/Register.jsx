import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css'; 
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import { Register } from '../Services/Users/Api';

function RegisterAplication() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);
    const [name, setName] = useState("");
    const [DataNascimento, setDataNascimento] = useState("");
    const [RendaMensal, setRendaMensal] = useState("");
    const navigate = useNavigate();

    const handleSubmit = async (event) => {
        event.preventDefault();
        setLoading(true);
        setError(null);
        try 
        {
            console.log(email, name, password);
            const data = await Register(email, name, password, DataNascimento, RendaMensal); 
            console.log(data);
            if(data.status === 200) {
                alert("Usuário cadastrado com sucesso");
                setEmail('');
                setPassword('');
                setName('');
                setDataNascimento('');
                setRendaMensal('');
                navigate("/");
            }
            else if (data.status === '409') {
                alert("Usuário ja cadastrado");
                setEmail('');
                setPassword('');
                setName('');
                setDataNascimento('');
                setRendaMensal('');
                return;
            }
        } 
        catch (error) 
        {
            setError(error.message);
        } 
        finally 
        {
            setLoading(false);
        }
    };

    return (
        <section className="vh-100 bg-dark">
            <div className="container-fluid h-custom">
                <div className="row d-flex justify-content-center align-items-center h-100">
                    <div className="col-md-6 col-lg-6 col-xl-4">
                        <form onSubmit={handleSubmit}>
                            <div className="d-flex flex-row align-items-center text-align-center justify-content-center">
                                <p className="lead fw-normal mb-2 me-2">Realize o seu Cadastro</p>
                            </div>

                            {error && <p className="text-danger">{error}</p>}
                            
                            <div className='form-outline mb-2 col-8 mx-auto'>
                                <label className="form-label" htmlFor="name">Nome</label>
                                <input 
                                    type="text" 
                                    id="name" 
                                    className="form-control form-control-sm" 
                                    placeholder="Insira o seu Nome" 
                                    value={name} 
                                    onChange={(e) => setName(e.target.value)} 
                                    required 
                                />
                            </div>

                            <div className="form-outline mb-2 col-8 mx-auto">
                                <label className="form-label" htmlFor="email">E-mail</label>
                                <input 
                                    type="email" 
                                    id="email" 
                                    className="form-control form-control-sm" 
                                    placeholder="Insira o seu E-mail" 
                                    value={email} 
                                    onChange={(e) => setEmail(e.target.value)} 
                                    required 
                                />
                            </div>
                            
                            <div className="form-outline mb-2 col-8 mx-auto">
                                <label className="form-label" htmlFor="password">Senha</label>
                                <input 
                                    type="password" 
                                    id="password" 
                                    className="form-control form-control-sm" 
                                    placeholder="Insira a sua senha" 
                                    value={password} 
                                    onChange={(e) => setPassword(e.target.value)} 
                                    required 
                                />
                            </div>

                             <div className="form-outline mb-2 col-8 mx-auto">
                                <label className="form-label" htmlFor="DataNascimento">Data Nascimento</label>
                                <input 
                                    type="date" 
                                    id="DataNascimento" 
                                    className="form-control form-control-sm" 
                                    value={DataNascimento} 
                                    onChange={(e) => setDataNascimento(e.target.value)} 
                                    required 
                                />
                            </div>

                             <div className="form-outline mb-2 col-8 mx-auto">
                                <label className="form-label" htmlFor="RendaMensal">Renda Mensal</label>
                                <input 
                                    type="text" 
                                    id="RendaMensal" 
                                    className="form-control form-control-sm" 
                                    placeholder="Insira a sua Média de Renda Mensal" 
                                    value={RendaMensal} 
                                    onChange={(e) => setRendaMensal(e.target.value)} 
                                    required 
                                />
                            </div>

                            <div className="form-outline mb-4 col-8 mx-auto">
                                <button 
                                    type="submit" 
                                    className="btn btn-primary btn-md btn-css w-100 text-white"
                                    disabled={loading}
                                >
                                    <b>{loading ? "Cadastrando..." : "Cadastrar"}</b>
                                </button>
                                <p className="small fw-bold mt-2 pt-1 mb-2 text-center">
                                    Possui uma conta? <a href='/' className="link-info">Entrar</a>
                                </p>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </section>
    );
}
export default RegisterAplication;