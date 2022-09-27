import { NavLink } from "react-router-dom";
import { HeaderProps } from "./Header.props";
import styles from "./Header.module.css";
import cn from "classnames";
import { Button, Menu } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../stores/store";
import LoginForm from "../../components/auth/LoginForm";
import RegisterForm from "../../components/auth/RegisterForm";
import { ModeratorMenu } from "../../components/menus/ModeratorMenu/ModeratorMenu";
import { AdminMenu } from "../../components/menus/AdminMenu/AdminMenu";
import { UserMenu } from "../../components/menus/UserMenu/UserMenu";

export const Header = observer(
  ({ className, ...props }: HeaderProps): JSX.Element => {
    const { userStore, modalStore } = useStore();
    const { isLoggedIn, user, logout } = userStore;
    const { openModal } = modalStore;

    const setMenu = (): JSX.Element => {
      if (user?.roles.includes("Admin")) {
        return <AdminMenu />;
      } else if (user?.roles.includes("Moderator")) {
        return <ModeratorMenu />;
      } else if (user?.roles.includes("User")) {
        return <UserMenu />;
      } else {
        return (
          <Menu.Item
            position="right"
            onClick={logout}
            text="Logout"
            icon="power"
          />
        );
      }
    };

    return (
      <div className={cn(className, styles.header)} {...props}>
        <Menu pointing>
          <Menu.Item as={NavLink} to="/" header name="TicketManagement" />
          <Menu.Item disabled header name="Venues" />
          <Menu.Item as={NavLink} to="/main" header name="Upcoming Events" />

          {isLoggedIn ? (
            setMenu()
          ) : (
            <Menu.Menu position="right">
              <Menu.Item>
                <Button primary onClick={() => openModal(<RegisterForm />)}>
                  Sign up
                </Button>
              </Menu.Item>

              <Menu.Item>
                <Button onClick={() => openModal(<LoginForm />)}>LogIn</Button>
              </Menu.Item>
            </Menu.Menu>
          )}
        </Menu>
      </div>
    );
  }
);
