import { Button, Segment, Form as FormUi } from "semantic-ui-react";
import { useState } from "react";
import { EventSeat } from "../../../../interfaces/event.interface";
import { useStore } from "../../../../stores/store";
import { observer } from "mobx-react-lite";
import { Formik, Form } from "formik";
import { TextInput } from "../../../formik/TextInput";
import SelectInput from "../../../formik/SelectInput";
import { eventSeatState } from "../../../../interfaces/options/eventSeatState";
import * as Yup from "yup";

export const EventSeatEditForm = observer((): JSX.Element => {
  const { eventStore } = useStore();
  const {
    selectedEventSeat,
    closeEditEventSeatForm,
    updateEventSeat,
    loading,
  } = eventStore;

  const initSelectEventSeat: EventSeat = selectedEventSeat ?? {
    id: 0,
    eventAreaId: 0,
    row: 0,
    number: 0,
    state: 0,
  };

  const [eventSeat] = useState(initSelectEventSeat);

  const handleFormSubmit = (eventSeat: EventSeat) => {
    updateEventSeat(eventSeat);
  };

  const validationSchema = Yup.object({
    row: Yup.number().integer().positive().required(),
    number: Yup.number().integer().positive().required(),
    state: Yup.number().integer().required(),
  });

  if (eventSeat === undefined) return <></>;

  return (
    <Segment clearing>
      <Formik
        validationSchema={validationSchema}
        enableReinitialize
        initialValues={eventSeat}
        onSubmit={(values) => handleFormSubmit(values)}
      >
        {({ handleSubmit, isValid, isSubmitting, dirty }) => (
          <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
            <FormUi.Group widths={3}>
              <TextInput placeholder="Row" label="Row" name="row" />
              <TextInput placeholder="Number" label="Number" name="number" />
              <SelectInput
                options={eventSeatState}
                placeholder="State"
                label="State"
                name="state"
              />
            </FormUi.Group>
            <Button
              disabled={isSubmitting || !dirty || !isValid}
              loading={loading}
              floated="right"
              positive
              type="submit"
              content="Save"
            />
            <Button
              onClick={closeEditEventSeatForm}
              floated="right"
              type="button"
              content="Cancel"
            />
          </Form>
        )}
      </Formik>
    </Segment>
  );
});
