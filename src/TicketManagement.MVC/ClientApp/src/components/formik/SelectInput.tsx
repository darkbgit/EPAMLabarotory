import { useField } from "formik";
import React from "react";
import { Form, Label, Select } from "semantic-ui-react";

interface Props {
  placeholder: string;
  name: string;
  options: any;
  onChange?: (value: any) => void;
  label?: string;
}

export const SelectInput = (props: Props) => {
  const [field, meta, helpers] = useField(props.name);
  return (
    <Form.Field error={meta.touched && !!meta.error}>
      <label>{props.label}</label>
      <Select
        {...field}
        clearable
        options={props.options}
        value={field.value || null}
        onChange={(e, d) => {
          console.log(d.value);
          helpers.setValue(d.value);
          props.onChange && props.onChange(d.value);
        }}
        onBlur={() => helpers.setTouched(true)}
        placeholder={props.placeholder}
        name={props.name}
      />
      {meta.touched && meta.error ? (
        <Label basic color="red">
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  );
};

export default SelectInput;
