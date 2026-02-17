import { Dropdown, Table } from "react-bootstrap";

const Lancamentos = () => {
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
                        <th>Data</th>
                        <th>Descrição</th>
                        <th>Valor</th>
                        <th>Categoria</th>
                        <th>Ações</th>
                    </tr>
                </thead>
                <tbody className="text-center">
                    <tr>
                        <td>01/01/2024</td>
                        <td>Salário</td>
                        <td>R$ 5.000,00</td>
                        <td>Recebimento</td>
                        <td>
                            <div className="d-flex justify-content-center gap-2">
                                <button className="btn btn-warning">Editar</button>
                                <button className="btn btn-danger">Deletar</button>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>05/01/2024</td>
                        <td>Aluguel</td>
                        <td>R$ 1.200,00</td>
                        <td>Despesa</td>
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