import axios from 'axios';
const api = axios.create({
    baseURL: "https://localhost:7202/api",
});
api.interceptors.request.use((config) => {
    const token = localStorage.getItem("AuthToken");
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
        return config;
    }
});
export const GetAllUsers = async () => {
    try 
    {
        const response = await api.get("/Users/GetAllUsers");
        return response.data;
    } 
    catch (error) 
    {
        return error;
    }
};
export const GetUserById = async (id) => {
    try 
    {
        const response = await api.get(`/Users/GetUserById/${id}`);
        return response;
    }
    catch (error) 
    {
        console.log(error);
        return error.response?.data;
    }  
};
export const UpdateUserById = async (id, email, name, password, dataNascimento,rendaMensal) => {
    try 
    {
        const response = await api.put(`/Users/UpdateUser/${id}`, {
            nome: name,
            email: email,   
            password: password,
            dataNascimento: dataNascimento,
            rendaMensal: rendaMensal
        });
        return response;
    }
    catch (error)
    {
        console.log(error);
        return error.response?.data;
    }
};
export const Register = async (email, name, password, dataNascimento, rendaMensal) => {
    try 
    {
        console.log(email, name, password, dataNascimento, rendaMensal);
        const response = await api.post("/Users/register", {
            nome: name,
            email: email,
            password: password,
            dataNascimento: dataNascimento,
            rendaMensal: rendaMensal
        });
        return response;
    } 
    catch (error)
    {
        console.log(error);
        return error.response?.data;
    }
};
export const DeleteUser = async (id) => {
    try 
    {
        const response = await api.delete(`/Users/DeleteUser/${id}`);
        return response;
    }
    catch (error)
    {
        console.log(error);
        return error.response?.data;
    }
};
export default api;