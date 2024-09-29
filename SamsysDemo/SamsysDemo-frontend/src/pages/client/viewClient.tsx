import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Col, Row } from "reactstrap";
import { ClientDTO } from "../../models/client/clientDTO";
import { MessagingHelper } from "../../models/helper/messagingHelper";
import { ClientService } from "../../services/clientService";

export default function ViewClient() {
    const { id } = useParams<{ id: string; }>();
    const [clientToView, setClientToView] = useState<ClientDTO | null>();    

    const clientService = new ClientService();

    const view = async () => {
        var resultGetClient: MessagingHelper<ClientDTO | null> = await clientService.Get(Number(id));

        if (resultGetClient.success == false) {
            return;
        }
        else
        {
            setClientToView(resultGetClient.obj!)
        }        
    }

   
    useEffect(() => {
        view();
    }, [id])

    return (
        <>
            <div style={{ width: "100%" }}>
                <Row>
                    <Col xl={12}>
                        <h1>Cliente</h1>
                    </Col>
                </Row>
            </div>
            {clientToView ? (
                <table className="table">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Nome</th>
                            <th>Contacto</th>
                            <th>Data Nascimento</th>
                            <th>Ativado?</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr key={clientToView.id}>
                            <td>{clientToView.id}</td>
                            <td>{clientToView.name}</td>
                            <td>{clientToView.phoneNumber}</td>
                            <td>{clientToView.dateOfBirth}</td>
                            <td>{clientToView.isActive ? "Sim" : "Não"}</td>
                        </tr>
                    </tbody>
                </table>
            ) : (
                <p>Erro ao obter o cliente pretendido! Tente mais tarde.</p>
            )}
          
        </>
    )
}