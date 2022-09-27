import React from "react";
import { Grid } from "semantic-ui-react";
import styles from "./EventSeatsBuy.module.css";
import cn from "classnames";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../stores/store";

export const EventSeatsBuy = observer((): JSX.Element => {
  const { eventPurchaseStore } = useStore();
  const { eventSeats, selectedEventSeats, selectEventSeat } =
    eventPurchaseStore;

  const drowSeatsGrid = (): JSX.Element => {
    const columns = Math.max(...eventSeats!.map((es) => es.number));
    const rows = Math.max(...eventSeats!.map((es) => es.row));
    return (
      <Grid centered>
        {[...Array(rows)].map((x, i) => (
          <Grid.Row key={i}>
            {`Row ${rows - i}`}
            {[...Array(columns)].map((y, z) => (
              <Grid.Column key={z}>
                {eventSeats.find(
                  (es) => es.number === z + 1 && es.row === rows - i
                ) && (
                  <button
                    disabled={
                      eventSeats.find(
                        (es) => es.number === z + 1 && es.row === rows - i
                      )?.state !== 0
                    }
                    className={cn(styles["button"], {
                      [styles["button-selected"]]:
                        selectedEventSeats.find(
                          (es) =>
                            es.id ===
                            eventSeats.find(
                              (es) => es.number === z + 1 && es.row === rows - i
                            )!.id
                        ) !== undefined,
                    })}
                    onClick={() =>
                      selectEventSeat(
                        eventSeats!.find(
                          (es) => es.number === z + 1 && es.row === rows - i
                        )!.id
                      )
                    }
                  >
                    {
                      eventSeats!.find(
                        (es) => es.number === z + 1 && es.row === rows - i
                      )?.number
                    }
                  </button>
                )}
              </Grid.Column>
            ))}
          </Grid.Row>
        ))}
      </Grid>
    );
  };
  if (eventSeats === undefined || eventSeats.length === 0) return <></>;
  return drowSeatsGrid();
});
