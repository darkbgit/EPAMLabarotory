import {
  LayoutModel,
  VenueModel,
} from "../../../../interfaces/event.interface";
import { useEffect, useState } from "react";
import SelectInput from "../../../formik/SelectInput";
import eventAgent from "../../../../helpers/api/eventAgent";
import { Form, Label } from "semantic-ui-react";

export const EventCreateSelectGroup = (): JSX.Element => {
  const [venues, setVenues] = useState<VenueModel[]>([]);

  const [layouts, setLayouts] = useState<LayoutModel[]>([]);

  const [selectedVenue, setSelectedVenue] = useState<VenueModel | undefined>(
    undefined
  );

  useEffect(() => {
    const doGetVenues = async () => {
      const venuesList = await eventAgent.Venues.venues();
      setVenues(venuesList);
    };

    doGetVenues();
  }, []);

  useEffect(() => {
    const doGetLayouts = async (venueId: number) => {
      const layouts = await eventAgent.Layouts.layouts(venueId);
      setLayouts(layouts);
    };

    if (selectedVenue !== undefined) {
      doGetLayouts(selectedVenue?.id);
    }
  }, [selectedVenue]);

  const selectVenue = (venueId: number) => {
    const venue = venues.find((v) => v.id === venueId);
    setSelectedVenue(venue);
  };

  return (
    <>
      <Form.Group widths={2}>
        <SelectInput
          placeholder=" Select Venue"
          label="Venue"
          name="venueId"
          onChange={(value) => selectVenue(value)}
          options={venues.map((v) => {
            return {
              text: v.description,
              value: v.id,
            };
          })}
        />
        <SelectInput
          placeholder="Select Layout"
          label="Layout"
          name="layoutId"
          options={layouts.map((l) => {
            return {
              text: l.description,
              value: l.id,
            };
          })}
        />
      </Form.Group>
      {selectedVenue && (
        <Label color="green">
          Venue time zone is {selectedVenue?.timeZoneId}. Set event time
          according to that time zone.
        </Label>
      )}
    </>
  );
};
