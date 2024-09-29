import { useEffect, useState } from "react";
import { ClientDTO } from "../../models/client/clientDTO";
import { MessagingHelper } from "../../models/helper/messagingHelper";
import { ClientService } from "../../services/clientService";

export default function GetAllPaginatedClient() {

    const [clientToList, setClientToList] = useState<ClientDTO[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [pageSize] = useState(10);

    const clientService = new ClientService();

    const getAllPaginated = async (pageNumber: number, pageSize: number) => {
        const resultGetClient: MessagingHelper<PaginatedResponse<ClientDTO>> = await clientService.GetAllPaginated(pageNumber,pageSize);

        if (resultGetClient.success == false) {
            return;
        }
        else
        {
            setClientToList(resultGetClient.obj.items);
            setCurrentPage(resultGetClient.obj.currentPage);
            setTotalPages(resultGetClient.obj.totalPages);
        }
    }

    useEffect(() => {
        getAllPaginated(currentPage, pageSize);
    }, [currentPage, pageSize]);

    return (
        <div>
          <h2>Clientes</h2>
          <table className="table">
            <thead>
              <tr>
                <th>Nome</th>
                <th>Contacto</th>
                <th>Data Nascimento</th>
                <th>Mais info</th>              
              </tr>
            </thead>
            <tbody>
            {clientToList.map(resultGetClient => {
                //I will remove the time from the dateTime in the razor page but it could be usefull for the user
                const dateOfBirth = new Date(resultGetClient.dateOfBirth).toLocaleDateString();
                return (
                <tr key={resultGetClient.id}>
                    <td>{resultGetClient.name}</td>
                    <td>{resultGetClient.phoneNumber}</td>
                    <td>{dateOfBirth}</td>
                    <td>
                      <a href={`/client/${resultGetClient.id}`}>Consultar</a>
                    </td>
                </tr>
                );
            })}
            </tbody>
          </table>
          <div className="pagination">
            {currentPage > 1 && (
              <button onClick={() => setCurrentPage(currentPage - 1)}>Anterior</button>
            )}
            {Array.from({ length: totalPages }, (_, index) => (
              <button
                key={index + 1}
                onClick={() => setCurrentPage(index + 1)}
                disabled={index + 1 === currentPage}
              >
                {index + 1}
              </button>
            ))}
            {currentPage < totalPages && (
              <button onClick={() => setCurrentPage(currentPage + 1)}>Próximo</button>
            )}
          </div>
        </div>
      )
}