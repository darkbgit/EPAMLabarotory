import { observer } from "mobx-react-lite";
import React from "react";
import { Link } from "react-router-dom";
import { Dropdown, Label, Menu } from "semantic-ui-react";
import { useStore } from "../../../stores/store";

export const UserMenu = observer((): JSX.Element => {
  const { userStore } = useStore();
  const { user, logout } = userStore;
  return (
    <>
      <Menu.Item position="right">
        <Label content={`Balance ${user?.balance} $`} />
      </Menu.Item>
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
            <Dropdown.Item
              as={Link}
              to={`/orders/${user?.id}`}
              text="Orders history"
            />
            <Dropdown.Item
              disabled
              as={Link}
              to="third-party"
              text="Top up balance"
            />
            <Dropdown.Item onClick={logout} text="Logout" icon="power" />
          </Dropdown.Menu>
        </Dropdown>
      </Menu.Item>
    </>
  );
});
