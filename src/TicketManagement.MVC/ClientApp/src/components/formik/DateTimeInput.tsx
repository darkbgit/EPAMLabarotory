import { useField } from "formik";
import React from "react";
import ReactDatePicker, { ReactDatePickerProps } from "react-datepicker";
import { Form, Label } from "semantic-ui-react";
import { registerLocale } from "react-datepicker";
import { be, enUS, ru } from "date-fns/locale";

registerLocale("be-BY", be);
registerLocale("en-US", enUS);
registerLocale("ru-RU", ru);

export const DateTimeInput = (props: Partial<ReactDatePickerProps>) => {
  const [field, meta, helpers] = useField(props.name!);
  return (
    <Form.Field error={meta.touched && !!meta.error}>
      <ReactDatePicker
        {...field}
        {...props}
        selected={(field.value && new Date(field.value)) || null}
        onChange={(value) => helpers.setValue(value)}
      />
      {meta.touched && meta.error ? (
        <Label basic color="red">
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  );
};

export default DateTimeInput;
