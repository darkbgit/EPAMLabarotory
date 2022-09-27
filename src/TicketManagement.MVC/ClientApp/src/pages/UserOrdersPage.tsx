import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Item } from "semantic-ui-react";
import { Page } from "../components";
import orderAgent from "../helpers/api/orderAgent";
import { OrderWithDetailsModel } from "../interfaces/order.interface";
import { Loading } from "../layout/Loading/Loading";

export const UserOrdersPage = () => {
  const [orders, setOrders] = useState<OrderWithDetailsModel[]>([]);
  const [loading, setLoading] = useState(false);

  const { id } = useParams<{ id: string }>();

  useEffect(() => {
    const doGetOrders = async (id: string) => {
      setLoading(true);
      const ordersList = await orderAgent.Orders.orders(id);
      setLoading(false);
      setOrders(ordersList);
    };

    if (id) {
      doGetOrders(id);
    }
  }, [id]);

  if (loading) return <Loading />;
  return (
    <Page title="Orders">
      {orders.length > 0 && (
        <Item.Group divided>
          {orders.map((o) => (
            <Item key={o.id}>
              <Item.Content>
                <Item.Header>{o.eventName}</Item.Header>
                <Item.Meta>{`${o.venueName}, ${o.layoutName} start at ${o.startDate}`}</Item.Meta>
                <Item.Description>{`Row-${o.row} Seat-${o.number}`}</Item.Description>
                <Item.Description>
                  {`Price ${o.price} $`} Details
                </Item.Description>
              </Item.Content>
            </Item>
          ))}
        </Item.Group>
      )}
    </Page>
  );
};

export default UserOrdersPage;
