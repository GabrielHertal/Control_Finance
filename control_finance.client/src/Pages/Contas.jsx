import { useEffect, useState } from "react";
import { Table, Button, Form, Pagination, Modal } from "react-bootstrap";
import { RegisterConta, GetAllContasByUserId, GetContaById, UpdateContaById, DeleteConta} from "../Services/Contas/api"; 
const Contas = () => {
    const [contas, setContas] = useState([]); // Lista de Contas
    const [showModal, setShowModal] = useState(false); // Controle do modal
    const [currentPage, setCurrentPage] = useState(1);
    const [newConta, setNewConta] = useState([]); 
    const CONTAS_PER_PAGE = 10; // Número de Contas por página
    const totalPages = Math.ceil(contas.length / CONTAS_PER_PAGE); 
    const startpageContas = Math.max(1, currentPage - Math.floor(CONTAS_PER_PAGE / 2));
    const endPageContas = Math.min(totalPages, startpageContas + CONTAS_PER_PAGE - 1); 

    const fetchContas = async () => {
        try {
            const res = await GetAllContasByUserId(localStorage.getItem("UserId"));
            if (res && Array.isArray(res)) {
                if(res[0].code === 404)
                {
                    alert(res[0].message);
                    setContas([]);
                    return;
                }
                setContas(res[0].data);
            } 
            else 
            {
                setContas([]);
                console.error("Erro: a API não retornou um array válido.");
            }
        } catch (error) {   
            console.error("Erro ao buscar Contas:", error);
            setContas([]);
        }
    };    
    useEffect(() => {
        fetchContas();
    }, []);
    // Criar novo Conta
    const handleCreateConta = async () => {
        try {
            if(newConta.titulo === "") {
                alert("Preencha todos os campos");
                return;
            }
            console.log(newConta);
            const data = await RegisterConta(newConta.titulo, newConta.tipo, localStorage.getItem("UserId"));
            if (data.status === 200) {
                alert("Conta cadastrado com sucesso");
                setShowModal(false);
                setNewConta({ titulo: "", tipo: ""});
                fetchContas();
            } 
            else if (data.status === 409) 
            {
                setNewConta({ titulo: "", tipo: ""});
                alert("Conta já cadastrado");
                return;
            }
        } catch (error) {
            console.log(error);
        }
    };
    // Editar Conta
    const handleEditConta = async (id) => {
        try{
            const ContaToEdit = await GetContaById(id);
            setNewConta({ id: ContaToEdit.data.id
                            , titulo: ContaToEdit.data.titulo
                            , fk_id_user: ContaToEdit.data.fk_Id_User
                            , tipo: ContaToEdit.data.tipo_Conta
                            , ativo: ContaToEdit.data.ativo });
            setShowModal(true);
        }
        catch (error) {
            console.log(error);
            alert(error);
        }
    };
    // Atualizar Conta
    const handleUpdateConta = async () => {
        try 
        {
            const data = await UpdateContaById(newConta.id, newConta.titulo, newConta.tipo, true);
            if (data.status === 200)
            {
                setShowModal(false);
                fetchContas();
            } 
            else if (data.status === '409') 
            {
                setNewConta({ titulo: "", tipo: ""});
                alert("Conta já cadastrada");
                return;
            }
        }
        catch (error) 
        {
            console.log(error);
            alert(error);
        }
    };
    // Deletar Conta
    const handleDeleteConta = async (id) => {
    try 
    {
        const data = await DeleteConta(id);
        console.log(data);
        if (data.status === 200) 
        {
            alert(data.data.message);           
            fetchContas();     
        }
    } catch (error)
    {
        console.log(error);
        alert(error);   
    }
    };
    return (
        <div className="container mt-4">
            <h1 className="text-center mb-4">Contas</h1>
            <div className="d-flex justify-content-end mb-3">
                <Button variant="primary" onClick={() => setShowModal(true)}>
                    + Criar Conta
                </Button>
            </div>
            {/* Tabela de Contas */}
            <Table borderless responsive="bg">
                <thead className="text-center">
                    <tr>
                        <th>ID</th>
                        <th>Conta</th>
                        <th>Ações</th>
                    </tr>
                </thead>
                <tbody className="text-center">
                    {contas.slice((currentPage - 1) * 10, currentPage * 10).map((Conta) => (
                        <tr key={Conta.id}>
                            <td>{Conta.id}</td>
                            <td>{Conta.titulo}</td>
                            <td className="align-align-middle">
                                <div className="d-flex justify-content-center gap-2">
                                    <Button variant="warning" onClick={() => handleEditConta(Conta.id)}>
                                        Editar
                                    </Button>
                                    <Button variant="danger" onClick={() => handleDeleteConta(Conta.id)}>
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
                {Array.from({ length: endPageContas - startpageContas + 1 }, (_, index) => (
                    <Pagination.Item
                        key={index}
                        active={index + startpageContas === currentPage}
                        onClick={() => setCurrentPage(index + startpageContas)}
                    >
                        {index + startpageContas}
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
                    setNewConta({ titulo: "", tipo: "" });
                }}
            >
                <Modal.Header closeButton>
                    <Modal.Title>{newConta.id ? "Editar Conta" : "Criar Nova Conta"}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3">
                            <Form.Label>Titulo</Form.Label>
                            <Form.Control
                                type="text"
                                value={newConta.titulo}
                                onChange={(e) => setNewConta({ ...newConta, titulo: e.target.value })}
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Tipo de Conta</Form.Label>
                            <Form.Select
                                onChange={(e) => setNewConta({ ...newConta, tipo: e.target.value })}
                                value={newConta.tipo}
                            > 
                                <option value="-1">Selecione o tipo de conta</option>
                                <option value="1">Físico</option>
                                <option value="2">Digital</option>
                                <option value="3">Investimento</option>
                            </Form.Select>
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button
                        variant="secondary"
                        onClick={() => {
                            setShowModal(false);
                            setNewConta({ titulo: "", tipo: "" });
                        }}
                    >
                        Fechar
                    </Button>
                    <Button variant="primary" onClick={newConta.id ? handleUpdateConta : handleCreateConta}>
                        Salvar
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};
export default Contas;