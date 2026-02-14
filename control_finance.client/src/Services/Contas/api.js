import axios from 'axios';
const api = axios.create({
    baseURL: "https://localhost:7202/api",
});
api.interceptors.request.use((config) => {
    const token = localStorage.getItem("AuthToken");
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});
export const GetAllContasByUserId = async (id) => {
    try
    {
        const response = await api.get(`/Contas/GetAllContasByUserId/${id}`);
        return response.data;
    }
    catch(error)
    {
        console.log(error);
        alert("Erro ao buscar contas");
        return error;
    }
};
export const GetContaById = async (id) => {
    try
    {
        const response = await api.get(`/Contas/GetContaById/${id}`);
        return response.data;
    }
    catch (error)
    {
        console.log(error);
        alert("Erro ao buscar conta");
        return error;
    }
};
export const UpdateContaById = async (id, titulo, tipo_conta, ativo) => {
    try
    {
        const response = await api.put(`/Contas/UpdateContaById/${id}`, {
            id: id,
            Titulo: titulo,
            tipo_conta: tipo_conta,
            ativo: ativo
        });
        return response;
    }
    catch (error)
    {
        console.log(error);
        alert("Erro ao atualizar conta");
        return error;
    }
};
export const RegisterConta = async (nome, id_user,tipo_conta) => {
    try
    {
        const response = await api.post("/Contas/CreateConta", {
            Titulo: nome,
            Tipo_Conta: tipo_conta,
            Fk_Id_User: id_user
        });
        return response;
    }
    catch (error)
    {
        console.log(error);
        alert("Erro ao cadastrar conta");
        return error;
    }
};
export const DeleteConta = async (id) => {
    try
    {
        const response = await api.delete(`/Contas/DeleteContaById/${id}`);
        return response;
    }
    catch (error)
    {
        console.log(error);
        alert("Erro ao deletar conta");
        return error;
    }
};
export default api;