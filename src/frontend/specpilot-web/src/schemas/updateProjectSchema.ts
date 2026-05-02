import { z } from 'zod';

export const updateProjectSchema = z.object({
  name: z.string().min(3, 'Informe um nome com pelo menos 3 caracteres.'),
  initialDescription: z.string().min(10, 'Descreva melhor a ideia inicial.'),
  goal: z.string().min(5, 'Informe um objetivo claro para o projeto.'),
  targetAudience: z.string().min(5, 'Informe o publico-alvo do projeto.')
});

export type UpdateProjectFormValues = z.infer<typeof updateProjectSchema>;
