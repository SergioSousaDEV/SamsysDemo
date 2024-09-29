import axios from "axios";
import { ClientDTO } from "../models/client/clientDTO";
import { ClientEditDTO } from "../models/client/clientEditDTO";
import { MessagingHelper } from "../models/helper/messagingHelper";
import { ClientCreateDTO } from "../models/client/clientCreateDTO";


var apiBaseUrl = process.env.REACT_APP_API_URL;
export class ClientService {
    async Get(id: number): Promise<MessagingHelper<ClientDTO | null>> {
        try {
            const result = await axios.get(`${apiBaseUrl}client/${id}`, {
                headers: {
                    Accept: "application/json",
                    "Content-Type": "application/json"
                },
            });

            return result.data;
        }
        catch (ex) {
            return new MessagingHelper<null>(false, "Ocorreu um erro inesperado ao obter o cliente", null)
        }
    }

    async GetAllPaginated(pageNumber: number,pageSize: number ): Promise<MessagingHelper<PaginatedResponse<ClientDTO>>> {
        try {
            const result = await axios.get(`${apiBaseUrl}client`, {
                headers: {
                    Accept: "application/json",
                    "Content-Type": "application/json"
                },
                params: {
                    pageNumber: pageNumber,
                    pageSize: pageSize
                }
            });

            return result.data;
        }
        catch (ex) {
            return new MessagingHelper<PaginatedResponse<ClientDTO>>(false, "Ocorreu um erro inesperado ao obter os clientes", createEmptyPaginatedResponse() )
        }
    }

    async CreateClient(dto: ClientCreateDTO): Promise<MessagingHelper<ClientDTO | null>> {
        try {
            const result = await axios.post(`${apiBaseUrl}client`,
                {
                    ...dto
                },
                {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "application/json"
                    },
                });

            return result.data;
        }
        catch (ex) {
            if (axios.isAxiosError(ex) && ex.response) {
                return new MessagingHelper<null>(false, ex.response.data.message, null)
            }
            else
            {
                return new MessagingHelper<null>(false, "Ocorreu um erro inesperado ao criar o cliente", null)
            }
        }
    }

    async Update(id: number, dto: ClientEditDTO): Promise<MessagingHelper<ClientDTO | null>> {
        try {
            const result = await axios.put(`${apiBaseUrl}client/${id}`,
                {
                    ...dto
                },
                {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "application/json"
                    },
                });

            return result.data;
        }
        catch (ex) {
            return new MessagingHelper<null>(false, "Ocorreu um erro inesperado ao atualizar o cliente", null)
        }
    }

    async Enable(id: number): Promise<MessagingHelper<null>> {
        try {
            const result = await axios.post(`${apiBaseUrl}client/${id}/Enable`,
                {},
                {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "application/json"
                    },
                });

            return result.data;
        }
        catch (ex) {
            return new MessagingHelper<null>(false, "Ocorreu um erro inesperado ao ativar o cliente", null)
        }
    }

    async Disable(id: number): Promise<MessagingHelper<null>> {
        try {
            const result = await axios.post(`${apiBaseUrl}client/${id}/Disable`,
                {},
                {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "application/json"
                    },
                });

            return result.data;
        }
        catch (ex) {
            return new MessagingHelper<null>(false, "Ocorreu um erro inesperado ao desativar o cliente", null)
        }
    }
}