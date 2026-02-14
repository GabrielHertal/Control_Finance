import axios from "axios";
const api = axios.create({
    baseURL: "https://localhost:7202/api",
});
export const login = async (email, password) => {
    try
    {
        const response = await api.post("/Security/Login", {
            email,
            password,
        });
        return response.data;
    } 
    catch (error)
    {
        alert(error);
    }  
}
export const GetUserInformation = async () => {
    try
    {
        const response = await api.get("/Security/GetUserInformation",{
            headers: 
            {
                Authorization: `Bearer ${localStorage.getItem("AuthToken")}`,
            },
        });
        return response.data.data;
    }
    catch (error) 
    {
        console.log(error);
        alert("Erro ao buscar informações do usuário, erro: " + error.message);
        return error;
    }
}
export default api;