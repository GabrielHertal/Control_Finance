import { Dropdown, Table } from "react-bootstrap";
import { GetAllLancamentosByUserId } from "../Services/Lancamentos/api";
import { useEffect, useState } from "react";

const Lancamentos = () => {
    const [Lancamento, setLancamento] = useState([]);

    const fetchLancamentos = async () => {
        try
        {
            const res = await GetAllLancamentosByUserId(localStorage.getItem("UserId"));
            if (res && Array.isArray(res))
            {
                setLancamento(res[0].data[0]);
            }
            else
            {
                setLancamento([]);
                console.error("Erro: a API não retornou um array válido." + res);
            }
        }
        catch (error)
        {
            console.error("Erro ao buscar lançamentos:", error);
            setLancamento([]);
        }
    };
    useEffect(() => {
        fetchLancamentos();
    }, []);

    return (
        <div className="container py-5">
            <div className="d-flex justify-content-end align-items-end mb-4">
                <Dropdown align="end">
                    <Dropdown.Toggle
                        className="rounded-pill px-4 py-2 fw-semibold border-0 shadow-sm"
                        style={{
                            background: "linear-gradient(135deg, #6f42c1, #4e2ca3)",
                            color: "white"
                        }}
                    >
                        + Novo Lançamento
                    </Dropdown.Toggle>
                    <Dropdown.Menu className="shadow rounded-4 border-0">
                        <Dropdown.Item>Recebimento</Dropdown.Item>
                        <Dropdown.Item>Despesa</Dropdown.Item>
                        <Dropdown.Item>Investimento</Dropdown.Item>
                        <Dropdown.Item>Parcela</Dropdown.Item>
                        <Dropdown.Divider />
                        <Dropdown.Item>Transferência</Dropdown.Item>
                    </Dropdown.Menu>
                </Dropdown>
            </div>
            <Table borderless responsive="bg" className="shadow-sm rounded-4">
                <thead className="text-center">
                    <tr>
                        <th>Data Vencimento</th>
                        <th>Título</th>
                        <th>Valor</th>
                        <th>Tipo</th>
                        <th>Ações</th>
                    </tr>
                </thead>
                <tbody className="text-center">
                    <tr key={Lancamento.id}>
                        <td>{Lancamento.data_Vencimento?.replace(/-/g, '/')}</td>
                        <td>{Lancamento.titulo}</td>
                        <td>{Lancamento.valor}</td>
                        <td>{Lancamento.tipo_LancamentoSTR}</td>
                        <td>
                            <div className="d-flex justify-content-center gap-2">
                                <button className="btn btn-warning">Editar</button>
                                <button className="btn btn-danger">Deletar</button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </Table>

        </div>
    );
};
export default Lancamentos;