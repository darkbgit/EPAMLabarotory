import styles from './PageTitle.module.css';
import { PageTitleProps } from './PageTitle.props';

export const PageTitle = ({ children }: PageTitleProps): JSX.Element => (
  <h2 className={styles.h2}>{children}</h2>
);
