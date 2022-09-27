import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { useParams } from "react-router-dom";
import { Button, Item } from "semantic-ui-react";
import { Page } from "../components";
import { EventAreasBuy } from "../components/buyFlow/EventAreasBuy/EventAreasBuy";
import { EventSeatsBuy } from "../components/buyFlow/EventSeatsBuy/EventSeatsBuy";
import { Loading } from "../layout/Loading/Loading";
import { useStore } from "../stores/store";

export const BuyTicketsPage = () => {
  const { eventPurchaseStore, userStore } = useStore();
  const {
    eventAreas,
    eventSeats,
    loadingInitial,
    loadEventAreas,
    loadEventSeats,
    buyTickets,
    selectedEventArea,
    selectedEventSeats,
    boughtTickets,
  } = eventPurchaseStore;
  const { id } = useParams();

  useEffect(() => {
    const idNumber = Number(id);
    loadEventAreas(idNumber);
  }, [id, loadEventAreas]);

  useEffect(() => {
    if (selectedEventArea !== undefined) {
      loadEventSeats(selectedEventArea.id);
    }
  }, [selectedEventArea, loadEventSeats]);

  if (loadingInitial) return <Loading />;
  return (
    <Page title="Buy tickets">
      {eventAreas && <EventAreasBuy />}
      {selectedEventArea && eventSeats.length > 0 && <EventSeatsBuy />}
      {userStore.isLoggedIn && userStore.user?.roles.find((r) => r === "User") && (
        <Button
          onClick={() => buyTickets(userStore.user!.id)}
          disabled={selectedEventSeats.length === 0}
        >
          Buy tickets
        </Button>
      )}
      {boughtTickets && (
        <Item.Group divided>
          {boughtTickets.map((o) => (
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

export default observer(BuyTicketsPage);
