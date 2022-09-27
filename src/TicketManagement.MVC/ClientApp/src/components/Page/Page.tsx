import { PageTitle } from '../PageTitle/PageTitle';
import styles from './Page.module.css';
import { PageProps } from './Page.props';

export const Page = ({ title, children }: PageProps): JSX.Element => (
  <div className={styles.div}>
    {title && <PageTitle>{title}</PageTitle>}
    {children}
  </div>
);
