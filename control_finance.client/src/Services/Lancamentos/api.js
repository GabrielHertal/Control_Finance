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
export const GetAllLancamentosByUserId = async (id) => {
    try 
    {
        const response = await api.get(`/Lancamentos/GetAllLancamentosByUserId/${id}`);
        return response.data;
    }
    catch (error)
    {
        console.log(error);
        alert("Erro ao buscar lançamentos");
        return error;
    }
};
export default api;