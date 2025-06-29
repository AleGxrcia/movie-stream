import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './styles.css';
import { Layout } from './components/layout/Layout.tsx';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Layout />
  </StrictMode>,
)
