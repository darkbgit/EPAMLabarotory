import React from "react";
import { Header, Icon, Segment } from "semantic-ui-react";

export const NotFound = () => {
  return (
    <Segment placeholder>
      <Header icon>
        <Icon name="search" />
        Not found
      </Header>
    </Segment>
  );
};

export default NotFound;
