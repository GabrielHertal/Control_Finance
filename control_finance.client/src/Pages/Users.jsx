import { useEffect, useState } from "react";
import { Table, Button, Form, Pagination, Modal } from "react-bootstrap";
import { Register, GetAllUsers, GetUserById, UpdateUserById, DeleteUser} from "../Services/Users/Api";
const Usuários = () => {
    const [users, setUsers] = useState([]); // Lista de usuários
    const [showModal, setShowModal] = useState(false); // Controle do modal
    const [newUser, setNewUser] = useState({ nome: "", email: "", password : "", datanascimento: "", rendaMensal: "" }); // Novo usuário
    const [currentPage, setCurrentPage] = useState(1);
    const USERS_PER_PAGE = 10; // Número de usuários por página
    const totalPages = Math.ceil(users.length / USERS_PER_PAGE); 
    const startpageusers = Math.max(1, currentPage - Math.floor(USERS_PER_PAGE / 2));
    const endPageUsers = Math.min(totalPages, startpageusers + USERS_PER_PAGE - 1); 

    // Carregar usuários da API
    const fetchUsers = async () => {
        try 
        {
            const res = await GetAllUsers();
            if (res && Array.isArray(res)) {
                setUsers(res[0].data);
            } else {
                setUsers([]);
                console.error("Erro: a API não retornou um array válido.");
            }
        } catch (error) {   
            console.error("Erro ao buscar usuários:", error);
            setUsers([]);
        }
    };    
    // Carregar usuários ao montar o componente
    useEffect(() => {
        fetchUsers();
    }, []);
    // Criar novo usuário
    const handleCreateUser = async () => {
        try {
            if(newUser.name === "" || newUser.email === "" || newUser.password === "") {
                alert("Preencha todos os campos");
                return;
            }
            const data = await Register(newUser.email, newUser.name, newUser.password, newUser.datanascimento, newUser.rendaMensal);
            if (data.status === '200') {
                alert("Usuário cadastrado com sucesso");
                setShowModal(false);
                fetchUsers();
            } else if (data.status === '409') {
                setNewUser({ name: "", email: "", password: "" });
                alert("Usuário já cadastrado");
                return;
            }
        } catch (error) {
            console.log(error);
        }
    };
    // Editar usuário
    const handleEditUser = async (id) => {
        try{
            const userToEdit = await GetUserById(id);
            setNewUser({ id: userToEdit.data.id
                       , name: userToEdit.data.nome
                       , email: userToEdit.data.email
                       , password: ""
                       , datanascimento: userToEdit.data.dataNascimento
                       ? userToEdit.data.dataNascimento.split('-').reverse().join('-')
                       : "", rendaMensal: userToEdit.data.rendaMensal });
            setShowModal(true);
        }
        catch (error) {
            console.log(error);
            alert(error);
        }
    };
    // Atualizar usuário
    const handleUpdateUser = async () => {
        try 
        {
            const data = await UpdateUserById(newUser.id, newUser.email, newUser.name, newUser.password, newUser.datanascimento, newUser.rendaMensal);
            if (data.status === 200)
            {
                setShowModal(false);
                fetchUsers();
                alert(data.data.message);
                if(newUser.id.toString() !== localStorage.getItem("UserId")) return;
                // localStorage.setItem("UserName", newUser.name);
                window.dispatchEvent(new Event("setUserNameLogged"));
            } 
            else if (data.status === 409) 
            {
                setNewUser({ name: "", email: "", password: "", datanascimento: "", rendaMensal: "" });
                alert(data.data.message);
                return;
            }
        }
        catch (error) 
        {
            console.log(error);
            alert(error);
        }
    };
    // Deletar usuário
    const handleDeleteUser = async (id) => {
        try {
            const data = await DeleteUser(id);
            if (data.status === 200) {
                alert("Usuário deletado com sucesso");           
                fetchUsers();     
            }
        } catch (error) {
            console.log(error);
        }
    };
    return (
        <div className="container mt-4">
            <h1 className="text-center mb-4">Usuários</h1>
            <div className="d-flex justify-content-end mb-3">
                <Button variant="primary" onClick={() => setShowModal(true)}>
                    + Criar Usuário
                </Button>
            </div>
            <Table borderless responsive="bg">
                <thead className="text-center">
                    <tr>
                        <th>ID</th>
                        <th>Nome</th>
                        <th>Email</th>
                        <th>Ações</th>
                    </tr>
                </thead>
                <tbody className="text-center">
                    {users.slice((currentPage - 1) * 10, currentPage * 10).map((user) => (
                        <tr key={user.id}>
                            <td>{user.id}</td>
                            <td>{user.nome}</td>
                            <td>{user.email}</td>
                            <td className="align-align-middle">
                                <div className="d-flex justify-content-center gap-2">
                                    <Button variant="warning" onClick={() => handleEditUser(user.id)}>
                                        Editar
                                    </Button>
                                    <Button variant="danger" onClick={() => handleDeleteUser(user.id)}>
                                        Deletar
                                    </Button>
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
            {/* Paginação */}
            <Pagination className="justify-content-center">
               <Pagination.Prev
                    disabled={currentPage === 1}
                    onClick={() => setCurrentPage((prev) => Math.max(prev - 1, 1))}
                />
                {Array.from({ length: endPageUsers - startpageusers + 1 }, (_, index) => (
                    <Pagination.Item
                        key={index}
                        active={index + startpageusers === currentPage}
                        onClick={() => setCurrentPage(index + startpageusers)}
                    >
                        {index + startpageusers}
                    </Pagination.Item>
                ))}
                <Pagination.Next
                    disabled={currentPage === totalPages}
                    onClick={() => setCurrentPage((prev) => Math.min(prev + 1, totalPages))}
                />
            </Pagination>
            {/* Modal de Cadastro */}
            <Modal
                show={showModal}
                onHide={() => {
                    setShowModal(false);
                    setNewUser({ name: "", email: "", password: "", datanascimento: "", rendaMensal: ""});
                }}
            >
                <Modal.Header closeButton>
                    <Modal.Title>{newUser.id ? "Editar Usuário" : "Criar Novo Usuário"}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3">
                            <Form.Label>Nome</Form.Label>
                            <Form.Control
                                type="text"
                                value={newUser.name}
                                onChange={(e) => setNewUser({ ...newUser, name: e.target.value })}
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Email</Form.Label>
                            <Form.Control
                                type="email"
                                value={newUser.email}
                                autoComplete="username"
                                onChange={(e) => setNewUser({ ...newUser, email: e.target.value })}
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Senha</Form.Label>
                            <Form.Control
                                type="password"
                                value={newUser.password}
                                autoComplete="new-password"
                                onChange={(e) => setNewUser({ ...newUser, password: e.target.value })}
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Data de Nascimento</Form.Label>
                            <Form.Control
                                type="date"
                                value={newUser.datanascimento}
                                onChange={(e) => setNewUser({ ...newUser, datanascimento: e.target.value })}
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Renda Mensal</Form.Label>
                            <Form.Control
                                type="number"
                                value={newUser.rendaMensal}
                                onChange={(e) => setNewUser({ ...newUser, rendaMensal: e.target.value })}
                            />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button
                        variant="secondary"
                        onClick={() => {
                            setShowModal(false);
                            setNewUser({ name: "", email: "", password: "", datanascimento: "", rendaMensal: "" });
                        }}
                    >
                        Fechar
                    </Button>
                    <Button variant="primary" onClick={newUser.id ? handleUpdateUser : handleCreateUser}>
                        Salvar
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};
export default Usuários;