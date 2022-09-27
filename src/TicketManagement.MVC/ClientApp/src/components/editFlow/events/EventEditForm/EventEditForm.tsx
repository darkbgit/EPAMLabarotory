import { Button, Segment, Form as FormUi, Label } from "semantic-ui-react";
import { useState } from "react";
import {
  EventModel,
  EventWithLayouts,
} from "../../../../interfaces/event.interface";
import { useStore } from "../../../../stores/store";
import { observer } from "mobx-react-lite";
import { Form, Formik } from "formik";
import TextInput from "../../../formik/TextInput";
import TextAreaInput from "../../../formik/TextAreaInput";
import SelectInput from "../../../formik/SelectInput";
import DateTimeInput from "../../../formik/DateTimeInput";
import * as Yup from "yup";
import { getDateWithoutTimezoneOffset } from "../../../../helpers/functions/dates";

export const EventEditForm = observer((): JSX.Element => {
  const { eventStore, userStore } = useStore();
  const { selectedEvent, closeEditForm, updateEvent, loading } = eventStore;

  const initSelectEvent: EventWithLayouts = {
    id: selectedEvent?.id ?? 0,
    name: selectedEvent?.name ?? "",
    description: selectedEvent?.description ?? "",
    startDate:
      selectedEvent !== undefined
        ? new Date(selectedEvent.startDate)
        : new Date(),
    endDate:
      selectedEvent !== undefined
        ? new Date(selectedEvent.endDate)
        : new Date(),
    imageUrl: selectedEvent?.imageUrl ?? "",
    layoutId: selectedEvent?.layoutId ?? 0,
    layouts: selectedEvent?.layouts ?? [],
    venueId: selectedEvent?.venueId ?? 0,
    venueTimeZoneId: selectedEvent?.venueTimeZoneId ?? "",
  };

  const validationShema = Yup.object({
    layoutId: Yup.number().required(),
    name: Yup.string().required(),
    description: Yup.string().required(),
    startDate: Yup.date().required(),
    endDate: Yup.date().required(),
    imageUrl: Yup.string().optional(),
  });

  const [event] = useState<EventWithLayouts>(initSelectEvent);

  const handleFormSubmit = (event: EventWithLayouts) => {
    const eventModel: EventModel = {
      id: event.id,
      name: event.name,
      description: event.description,
      startDate: new Date(getDateWithoutTimezoneOffset(event.startDate)),
      endDate: new Date(getDateWithoutTimezoneOffset(event.endDate)),
      imageUrl: event.imageUrl,
      layoutId: event.layoutId,
    };

    updateEvent(eventModel);
  };

  if (event === undefined) return <></>;

  return (
    <Segment clearing>
      <Formik
        validationSchema={validationShema}
        enableReinitialize
        initialValues={event}
        onSubmit={(values) => handleFormSubmit(values)}
      >
        {({ handleSubmit, isSubmitting, isValid, dirty }) => (
          <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
            <FormUi.Group widths={2}>
              <SelectInput
                placeholder="Select Layout"
                label="Layout"
                name="layoutId"
                options={event.layouts.map((l) => {
                  return {
                    text: l.description,
                    value: l.id,
                  };
                })}
              />
              <TextInput placeholder="Name" label="Name" name="name" />
            </FormUi.Group>
            <TextAreaInput
              label="Description"
              placeholder="Description"
              name="description"
              rows={3}
            />
            <Label>Event Date time zone is {event.venueTimeZoneId}</Label>
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
              loading={loading}
              floated="right"
              positive
              type="submit"
              content="Save"
            />
            <Button
              onClick={closeEditForm}
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
