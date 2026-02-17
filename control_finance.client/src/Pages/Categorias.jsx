import { useEffect, useState } from "react";
import { Table, Button, Form, Pagination, Modal } from "react-bootstrap";
import { RegisterCategoria, GetAllCategoriasByUserId, GetCategoriaById, UpdateCategoriaById, DeleteCategoria} from "../Services/Categorias/api";
const Categorias = () => {
    const [categorias, setCategorias] = useState([]); // Lista de categorias
    const [showModal, setShowModal] = useState(false); // Controle do modal
    const [currentPage, setCurrentPage] = useState(1);
    const [newCategoria, setNewCategoria] = useState([]); 
    const CATEGORIAS_PER_PAGE = 10; // Número de categorias por página
    const totalPages = Math.ceil(categorias.length / CATEGORIAS_PER_PAGE); 
    const startpageCategorias = Math.max(1, currentPage - Math.floor(CATEGORIAS_PER_PAGE / 2));
    const endPageCategorias = Math.min(totalPages, startpageCategorias + CATEGORIAS_PER_PAGE - 1); 

    const fetchCategorias = async () => {
        try 
        {
            const res = await GetAllCategoriasByUserId(localStorage.getItem("UserId"));
            if (res && Array.isArray(res)) 
            {
                setCategorias(res[0].data);
            } 
            else 
            {
                setCategorias([]);
                console.error("Erro: a API não retornou um array válido." + res);
            }
        } 
        catch (error) 
        {   
            console.error("Erro ao buscar categorias:", error);
            setCategorias([]);
        }
    };    
    useEffect(() => {
        fetchCategorias();
    }, []);
    // Criar novo categoria
    const handleCreateCategoria = async () => {
        try {
            if(newCategoria.nome === "") 
            {
                alert("Preencha todos os campos");
                return;
            }
            const data = await RegisterCategoria(newCategoria.nome, localStorage.getItem("UserId"));
            if (data.status === 200) 
            {
                alert("Categoria cadastrado com sucesso");
                setShowModal(false);
                setNewCategoria({ nome: "", tipo: ""});
                fetchCategorias();
            } 
            else if (data.status === 409) 
            {
                setCategorias({ nome: "", tipo: ""});
                alert("categoria já cadastrado");
                return;
            }
        } 
        catch (error) 
        {
            console.error("Erro ao criar categorias:", error);
            alert("Erro ao criar categoria");
        }
    };
    // Editar categoria
    const handleEditCategoria = async (id) => {
        try{
            const CategoriaToEdit = await GetCategoriaById(id);
            setNewCategoria({ id: CategoriaToEdit.data.id
                            , nome: CategoriaToEdit.data.nome
                            , fk_id_user: CategoriaToEdit.data.fkIdUser
                            , ativo: CategoriaToEdit.data.ativo });
            setShowModal(true);
        }
        catch (error) {
            console.log(error);
            alert(error);
        }
    };
    // Atualizar categoria
    const handleUpdateCategoria = async () => {
        try 
        {
            const data = await UpdateCategoriaById(newCategoria.id, newCategoria.nome, newCategoria.fk_id_user, true);
            console.log(data);
            if (data.status === 200)
            {
                setShowModal(false);
                fetchCategorias();
            } 
            else if (data.status === '409') 
            {
                setNewCategoria({ nome: "", tipo: ""});
                alert("Categoria já cadastrada");
                return;
            }
        }
        catch (error) 
        {
            console.error("Erro ao atualizar categorias:", error);
            alert("Erro ao atualizar categoria");
        }
    };
    // Deletar categoria
    const handleDeleteCategoria = async (id) => {
        try 
        {
            const data = await DeleteCategoria(id);
            console.log(data);
            if (data.status === 200) 
            {
                alert(data.data.message);           
                fetchCategorias();     
            }
        } 
        catch (error)
        {
            console.error("Erro ao deletar categorias:", error);
            alert("Erro ao deletar categoria");
        }
    };
    return (
        <div className="container mt-4">
            <h1 className="text-center mb-4">Categorias</h1>
            <div className="d-flex justify-content-end mb-3">
                <Button variant="primary" onClick={() => setShowModal(true)}>
                    + Criar Categoria
                </Button>
            </div>
            {/* Tabela de categorias */}
            <Table borderless responsive="bg">
                <thead className="text-center">
                    <tr>
                        <th>ID</th>
                        <th>Categoria</th>
                        <th>Ações</th>
                    </tr>
                </thead>
                <tbody className="text-center">
                    {categorias.slice((currentPage - 1) * 10, currentPage * 10).map((categoria) => (
                        <tr key={categoria.id}>
                            <td>{categoria.id}</td>
                            <td>{categoria.nome}</td>
                            <td className="align-align-middle">
                                <div className="d-flex justify-content-center gap-2">
                                    <Button variant="warning" onClick={() => handleEditCategoria(categoria.id)}>
                                        Editar
                                    </Button>
                                    <Button variant="danger" onClick={() => handleDeleteCategoria(categoria.id)}>
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
                {Array.from({ length: endPageCategorias - startpageCategorias + 1 }, (_, index) => (
                    <Pagination.Item
                        key={index}
                        active={index + startpageCategorias === currentPage}
                        onClick={() => setCurrentPage(index + startpageCategorias)}
                    >
                        {index + startpageCategorias}
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
                    setNewCategoria({ nome: "", tipo: "" });
                }}
            >
                <Modal.Header closeButton>
                    <Modal.Title>{newCategoria.id ? "Editar Categoria" : "Criar Nova Categoria"}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3">
                            <Form.Label>Nome</Form.Label>
                            <Form.Control
                                type="text"
                                value={newCategoria.nome}
                                onChange={(e) => setNewCategoria({ ...newCategoria, nome: e.target.value })}
                            />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button
                        variant="secondary"
                        onClick={() => {
                            setShowModal(false);
                            setNewCategoria({ nome: "", tipo: "" });
                        }}
                    >
                        Fechar
                    </Button>
                    <Button variant="primary" onClick={newCategoria.id ? handleUpdateCategoria : handleCreateCategoria}>
                        Salvar
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};
export default Categorias;