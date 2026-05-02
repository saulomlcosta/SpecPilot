import { z } from 'zod';

export const answerQuestionItemSchema = z.object({
  questionId: z.string().min(1),
  answer: z.string().trim().min(1, 'Resposta obrigatoria.')
});

export const answerQuestionsSchema = z.object({
  answers: z.array(answerQuestionItemSchema).min(1, 'Nenhuma pergunta encontrada para resposta.')
});

export type AnswerQuestionsFormValues = z.infer<typeof answerQuestionsSchema>;
