import { observer } from "mobx-react-lite";
import React from "react";
import { Link } from "react-router-dom";
import { Dropdown, Menu } from "semantic-ui-react";
import { useStore } from "../../../stores/store";

export const ModeratorMenu = observer((): JSX.Element => {
  const { userStore } = useStore();
  const { user, logout } = userStore;
  return (
    <Menu.Item position="right">
      <Dropdown pointing="top left" text={user?.userName}>
        <Dropdown.Menu>
          <Dropdown.Item
            disabled
            as={Link}
            to={`/profiles/${user?.id}`}
            text="My Profile"
            icon="user"
          />
          <Dropdown.Item as={Link} to="edit-list" text="Edit events" />
          <Dropdown.Item
            disabled
            as={Link}
            to="third-party"
            text="Load from third party manager"
          />
          <Dropdown.Item onClick={logout} text="Logout" icon="power" />
        </Dropdown.Menu>
      </Dropdown>
    </Menu.Item>
  );
});
