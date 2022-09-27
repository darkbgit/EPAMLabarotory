import { ErrorMessage, Form, Formik } from "formik";
import { observer } from "mobx-react-lite";
import React from "react";
import { Button, Header, Label } from "semantic-ui-react";
import { useStore } from "../../stores/store";
import TextInput from "../formik/TextInput";

export const LoginForm = observer(() => {
  const { userStore } = useStore();
  const { login } = userStore;
  return (
    <Formik
      initialValues={{ userNameOrEmail: "", password: "", error: null }}
      onSubmit={(values, { setErrors }) =>
        login(values).catch((error) =>
          setErrors({ error: "Invalid Email or password" })
        )
      }
    >
      {({ handleSubmit, isSubmitting, errors }) => (
        <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
          <Header as="h2" content="Login" textAlign="center" />
          <TextInput name="userNameOrEmail" placeholder="User name or email" />
          <TextInput name="password" placeholder="Password" type="password" />
          <ErrorMessage
            name="error"
            render={() => <Label basic color="red" content={errors.error} />}
          />
          <Button
            loading={isSubmitting}
            positive
            content="Login"
            type="submit"
            fluid
          />
        </Form>
      )}
    </Formik>
  );
});

export default LoginForm;
