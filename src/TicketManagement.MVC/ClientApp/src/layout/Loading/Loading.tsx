import { LoadingProps } from "./Loading.props";
import { Dimmer, Loader } from "semantic-ui-react";

export const Loading = ({
  content = "Loading...",
}: LoadingProps): JSX.Element => {
  return (
    <Dimmer active={true}>
      <Loader content={content} />
    </Dimmer>
  );
};
