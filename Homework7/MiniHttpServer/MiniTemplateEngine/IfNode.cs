namespace MiniTemplateEngine;

public class IfNode: Node
{
    public string Condition { get; }
    public List<Node> TrueBranch { get; }
    public List<Node> FalseBranch { get; }  
    
    public IfNode(string condition, List<Node> trueBranch, List<Node> falseBranch)
    {
        Condition = condition;
        TrueBranch = trueBranch;
        FalseBranch = falseBranch;
    }

    public override string Render(object data)
    {
        bool cond = EvalCondition(Condition, data);

        var branch = cond ? TrueBranch : FalseBranch;

        return string.Join("", branch.Select(n => n.Render(data)));
    }

    private static bool EvalCondition(string condition, object data)
    {
        condition = condition.Trim();
        bool negate = false;

        if (condition.StartsWith("!"))
        {
            negate = true;
            condition = condition.Substring(1).Trim();
        }

        var val = VariableNode.GetValueFromExpression(condition, data);

        bool result = val switch
        {
            bool b => b,   // Если bool — берем как есть
            null => false, // Если null — ложь
            _ => true      // Всё остальное считаем истиной
        };

        return negate ? !result : result;
    }
}