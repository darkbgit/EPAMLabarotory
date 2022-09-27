import React from "react";
import { Grid } from "semantic-ui-react";
import styles from "./EventAreasBuy.module.css";
import cn from "classnames";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../stores/store";

export const EventAreasBuy = observer((): JSX.Element => {
  const { eventPurchaseStore } = useStore();
  const { eventAreas, selectedEventArea, selectEventArea } = eventPurchaseStore;

  const drowAreasGrid = (): JSX.Element => {
    const columns = Math.max(...eventAreas!.map((ea) => ea.coordX)) + 1;
    const rows = Math.max(...eventAreas!.map((ea) => ea.coordY)) + 1;
    return (
      <Grid centered>
        {[...Array(rows)].map((x, i) => (
          <Grid.Row key={i}>
            {[...Array(columns)].map((y, z) => (
              <Grid.Column width={2} key={z} className={cn(styles["column"])}>
                {
                  <button
                    disabled={
                      eventAreas.find(
                        (ea) => ea.coordX === z && ea.coordY === rows - (i + 1)
                      ) === undefined
                    }
                    className={cn(styles["button"], {
                      [styles["button-selected"]]:
                        selectedEventArea &&
                        eventAreas.find(
                          (ea) =>
                            ea.coordX === z && ea.coordY === rows - (i + 1)
                        )?.id === selectedEventArea.id,
                    })}
                    onClick={() => {
                      selectEventArea(
                        eventAreas!.find(
                          (ea) =>
                            ea.coordX === z && ea.coordY === rows - (i + 1)
                        )!.id
                      );
                    }}
                  >
                    {
                      eventAreas!.find(
                        (ea) => ea.coordX === z && ea.coordY === rows - (i + 1)
                      )?.description
                    }
                  </button>
                }
              </Grid.Column>
            ))}
          </Grid.Row>
        ))}
      </Grid>
    );
  };
  if (eventAreas === undefined || eventAreas.length === 0) return <></>;
  return drowAreasGrid();
});
