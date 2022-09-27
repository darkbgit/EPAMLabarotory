import {
  OrderBoughtModel,
  OrderWithDetailsModel,
} from "../../interfaces/order.interface";
import { createAxios, requests } from "./agentFactory";

const baseUrl = process.env.REACT_APP_ORDER_MANAGEMENT;

const orderAgentInstance = createAxios(baseUrl!);

const Orders = {
  buy: (ticket: OrderBoughtModel) =>
    requests.post<OrderWithDetailsModel>(orderAgentInstance, "/orders", ticket),
  orders: (id: string) =>
    requests.get<OrderWithDetailsModel[]>(
      orderAgentInstance,
      `/orders/with-ticket-info-by-user-id/${id}`
    ),
};

const orderAgent = {
  Orders,
};

export default orderAgent;
