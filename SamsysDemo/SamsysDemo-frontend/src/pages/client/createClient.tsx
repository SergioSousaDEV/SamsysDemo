import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Col, Row } from "reactstrap";
import { ClientDTO } from "../../models/client/clientDTO";
import { ClientCreateDTO } from "../../models/client/clientCreateDTO";
import { MessagingHelper } from "../../models/helper/messagingHelper";
import { ClientService } from "../../services/clientService";
import ClientStatusComponent from "../../components/client/statusComponent";

export default function CreateClient() {

    const resultCreateClient: MessagingHelper<ClientDTO | null> = new MessagingHelper<ClientDTO | null>(false,"",null);
    const [clientToCreate, setClientToCreate] = useState<ClientCreateDTO>({
        name: '',
        phoneNumber: '',
        isActive: true,
        dateOfBirth: null,
    });
        
    const [errorMessage, setErrorMessage] = useState<string>();
    const [successMessage, setSuccessMessage] = useState<string>();

    const clientService = new ClientService();

    const create = async () => {

    const resultCreateClient: MessagingHelper<ClientDTO | null> = await clientService.CreateClient(clientToCreate);

        if (resultCreateClient.success == false) {
            setErrorMessage(resultCreateClient.message);
            setSuccessMessage("");
            return;
        }
        else
        {
            setSuccessMessage("Cliente criado com sucesso! ID : " + resultCreateClient.obj?.id);
            setErrorMessage("");            
            setClientToCreate({
                name: '',
                phoneNumber: '',
                isActive: true,
                dateOfBirth: null,
            });
        }
    }

    useEffect(() => {
      
    }, [])

    return (
        <>
            <div style={{ width: "100%" }}>
                <Row>
                    <Col xl={12}>
                        <h1>Criar Cliente</h1>
                    </Col>
                </Row>
            </div>


            <div style={{ width: "20%", marginTop: "2em", display: "inline-block" }}>
                <Row>
                    <Col xl={6} style={{ textAlign: "right" }}>
                        <label>Nome: </label>
                    </Col>
                    <Col xl={6}>
                        <input type="text"
                            value={clientToCreate?.name ?? ""}
                            onChange={(e: React.ChangeEvent<HTMLInputElement>) => setClientToCreate({ ...clientToCreate, name: e.target.value})} />
                    </Col>
                </Row>

                <Row>
                    <Col xl={6} style={{ textAlign: "right" }}>
                        <label>Contacto: </label>
                    </Col>
                    <Col xl={6}>
                        <input type="text"
                            value={clientToCreate?.phoneNumber ?? ""}
                            onChange={(e: React.ChangeEvent<HTMLInputElement>) => setClientToCreate({ ...clientToCreate, phoneNumber: e.target.value })} />
                    </Col>
                </Row>   

                <Row>
                    <Col xl={6} style={{ textAlign: "right" }}>
                        <label>Data de Nascimento: </label>
                    </Col>
                    <Col xl={6}>
                        <input type="date"
                            value={clientToCreate?.dateOfBirth ?? ""}
                            onChange={(e: React.ChangeEvent<HTMLInputElement>) => setClientToCreate({ ...clientToCreate, dateOfBirth: e.target.value })} />
                    </Col>
                </Row>      

                <Row>
                    <Col xl={12}>
                        <button className="btnCreateClient"
                            onClick={create}>
                            Criar Cliente
                        </button>
                    </Col>
                </Row>

                {errorMessage &&
                    <Row>
                        <Col xl={12} className="error">
                            {errorMessage}
                        </Col>
                    </Row>
                }

                {successMessage &&
                    <Row>
                        <Col xl={12} className="success">
                            {successMessage}
                        </Col>
                    </Row>
                }
            </div>
        </>
    )
}