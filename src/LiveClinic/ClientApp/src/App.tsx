import React from 'react';
import './App.css';
import {QueryClient, QueryClientProvider} from "react-query";

const  queryClient=new QueryClient()
function App() {
  return (
    <QueryClientProvider client={queryClient}>
      
    </QueryClientProvider>
  );
}

export default App;
