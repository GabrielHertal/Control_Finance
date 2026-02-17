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

export const GetAllCategoriasByUserId = async (id) => {
    try 
    {
        const response = await api.get(`/Categorias/GetAllCategoriasByUserId/${id}`);
        return response.data;
    }
    catch (error)
    {
        console.log(error);
        alert("Erro ao buscar categorias");
        return error;
    }
};

export const GetCategoriaById = async (id) => {
    try
    {
        const response = await api.get(`/Categorias/GetCategoriaById/${id}`);
        return response.data;
    }
    catch (error)
    {
        console.log(error);
        alert("Erro ao buscar categoria");
        return error;
    }  
};
export const UpdateCategoriaById = async (id, nome, id_user, ativo) => {
    try
    {
        const response = await api.put(`/Categorias/UpdateCategoriaById/${id}`, {
            id: id,
            nome: nome, 
            FkIdUser: id_user,
            ativo: ativo
        });
        return response;
    }
    catch (error)
    {
        console.log(error);
        return error.response?.data;
    }
};
export const RegisterCategoria = async (nome, id_user) => {
    try
    {
        const response = await api.post("/Categorias/CreateCategoria", {
            nome: nome,
            FkIdUser: id_user
        });
        console.log(response);
        return response;
    }
    catch (error)
    {
        console.log(error);
        return error.response?.data;
    }
};

export const DeleteCategoria = async (id) => {
    try
    {       
        const response = await api.delete(`/Categorias/DeleteCategoriaById/${id}`);
        return response;
    }
    catch (error)
    {
        console.log(error);
        return error.response?.data;
    }
};

export default api;