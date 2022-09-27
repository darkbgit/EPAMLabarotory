import { Button, Form as FormUi, Segment } from "semantic-ui-react";
import { useStore } from "../../../../stores/store";
import { observer } from "mobx-react-lite";
import {
  CreateEventModel,
  EventWithLayouts,
} from "../../../../interfaces/event.interface";
import * as Yup from "yup";
import { Form, Formik } from "formik";

import TextInput from "../../../formik/TextInput";
import TextAreaInput from "../../../formik/TextAreaInput";
import DateTimeInput from "../../../formik/DateTimeInput";
import { EventCreateSelectGroup } from "../EventCreateSelectGroup/EventCreateSelectGroup";
import { useState } from "react";
import { getDateWithoutTimezoneOffset } from "../../../../helpers/functions/dates";

export const EventCreateForm = observer((): JSX.Element => {
  const { eventStore, userStore } = useStore();
  const { setCreateEventMode, createEvent, loading } = eventStore;

  const initSelectEvent: EventWithLayouts = {
    id: 0,
    name: "",
    description: "",
    startDate: new Date(),
    endDate: new Date(),
    imageUrl: "",
    layoutId: -1,
    layouts: [],
    venueId: -1,
    venueTimeZoneId: "",
  };

  const validationShema = Yup.object({
    layoutId: Yup.number().required(),
    name: Yup.string().required(),
    description: Yup.string().required(),
    startDate: Yup.date().required(),
    endDate: Yup.date().required(),
    imageUrl: Yup.string().optional(),
  });

  const [newEvent] = useState(initSelectEvent);

  const handleFormSubmit = (event: EventWithLayouts) => {
    const eventModel: CreateEventModel = {
      name: event.name,
      description: event.description,
      startDate: new Date(getDateWithoutTimezoneOffset(event.startDate)),
      endDate: new Date(getDateWithoutTimezoneOffset(event.endDate)),
      imageUrl: event.imageUrl,
      layoutId: event.layoutId,
    };

    createEvent(eventModel);
  };

  return (
    <Segment clearing>
      <Formik
        validationSchema={validationShema}
        enableReinitialize={true}
        initialValues={newEvent}
        onSubmit={(values) => handleFormSubmit(values)}
      >
        {({ handleSubmit, isSubmitting, isValid, dirty, values }) => (
          <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
            <EventCreateSelectGroup />
            <TextInput label="Name" placeholder="Name" name="name" />
            <TextAreaInput
              label="Description"
              placeholder="Description"
              name="description"
              rows={3}
            />
            <FormUi.Group widths={2}>
              <DateTimeInput
                placeholderText="Start date"
                name="startDate"
                showTimeSelect
                timeCaption="Time"
                dateFormat="Pp"
                locale={userStore.user?.language}
              />
              <DateTimeInput
                placeholderText="End date"
                name="endDate"
                showTimeSelect
                timeCaption="time"
                dateFormat="Pp"
                locale={userStore.user?.language}
              />
            </FormUi.Group>
            <TextAreaInput
              label="Image URL"
              placeholder="Image URL"
              name="imageUrl"
              rows={3}
            />
            <Button
              disabled={isSubmitting || !dirty || !isValid}
              floated="right"
              positive
              type="submit"
              content="Create"
              loading={loading}
            />
            <Button
              onClick={() => setCreateEventMode(false)}
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
