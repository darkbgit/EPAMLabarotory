import { Button, Form as FormUi, Segment } from "semantic-ui-react";
import { useState } from "react";
import { EventArea } from "../../../../interfaces/event.interface";
import { useStore } from "../../../../stores/store";
import { observer } from "mobx-react-lite";
import { Form, Formik } from "formik";
import TextInput from "../../../formik/TextInput";
import * as Yup from "yup";

export const EventAreaEditForm = observer((): JSX.Element => {
  const { eventStore } = useStore();
  const {
    selectedEventArea,
    closeEditEventAreaForm,
    updateEventArea,
    loading,
  } = eventStore;

  const initSelectEventArea: EventArea = selectedEventArea ?? {
    id: 0,
    eventId: 0,
    description: "",
    coordX: 0,
    coordY: 0,
  };

  const validationSchema = Yup.object({
    description: Yup.string().required(),
    coordX: Yup.number().integer().min(0).required(),
    coordY: Yup.number().integer().min(0).required(),
    price: Yup.number().min(0).optional(),
  });

  const [eventArea] = useState(initSelectEventArea);

  const handleFormSubmit = (eventArea: EventArea) => {
    updateEventArea(eventArea);
  };

  if (eventArea === undefined) return <></>;

  return (
    <Segment clearing>
      <Formik
        validationSchema={validationSchema}
        enableReinitialize
        initialValues={eventArea}
        onSubmit={(values) => handleFormSubmit(values)}
      >
        {({ handleSubmit, isSubmitting, isValid, dirty }) => (
          <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
            <FormUi.Group widths={2}>
              <TextInput
                placeholder="Description"
                label="Description"
                name="description"
              />
              <TextInput placeholder="Price" label="Price" name="price" />
            </FormUi.Group>
            <FormUi.Group widths={2}>
              <TextInput
                placeholder="Coordinate X"
                label="CoordX"
                name="coordX"
              />
              <TextInput
                placeholder="Coordinate Y"
                label="CoordY"
                name="coordY"
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
              onClick={closeEditEventAreaForm}
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
