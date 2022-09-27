import React from "react";
import { withLayout } from "../layout/Layout";
import { Page } from "../components";
import { useForm } from "react-hook-form";

type FormData = {
  title: string;
  content: string;
};

export const ThirdPartyLoadPage = () => {
  const [successfullySubmited, setSuccessfullySubmited] = React.useState(false);
  const {
    register,
    handleSubmit,
    formState,
    formState: { errors },
  } = useForm<FormData>({
    mode: "onBlur",
  });
  const submitForm = async (data: FormData) => {
    const result = true;
    // const result = await postFilePath({
    //   filePath: data.filePath
    // });
    setSuccessfullySubmited(result ? true : false);
  };

  return (
    <Page title="Load event from file">
      <form onSubmit={handleSubmit(submitForm)}>
        <fieldset disabled={formState.isSubmitting || successfullySubmited}>
          <div>
            <label htmlFor="title">Title</label>
            <input
              id="title"
              type="text"
              {...register("title", {
                required: true,
                minLength: 10,
              })}
            />
            {errors.title && errors.title.type === "required" && (
              <div>You must enter the question title</div>
            )}
            {errors.title && errors.title.type === "minLength" && (
              <div>The title musy be at least 10 characters</div>
            )}
          </div>
          <div>
            <label htmlFor="content">Content</label>
            <textarea
              id="content"
              {...register("content", {
                required: true,
                minLength: 50,
              })}
            />
            {errors.content && errors.content.type === "required" && (
              <div>You must enter the question content</div>
            )}
            {errors.content && errors.content.type === "minLength" && (
              <div>The content musy be at least 50 characters</div>
            )}
          </div>
          <div>
            <button type="submit">Load file</button>
          </div>
          {successfullySubmited && (
            <div>Your question was successfully submited</div>
          )}
        </fieldset>
      </form>
    </Page>
  );
};

export default withLayout(ThirdPartyLoadPage);
