using System;

public class GameObjectProperties{

	public static int MaxSize = 20;
	public byte[] PlayerID, PositionX, PositionY, Horizontal, Vertical, IsMoving, IsAttacking;


	public GameObjectProperties()
    {
		this.PlayerID = new byte[sizeof(short)];
        this.PositionX = new byte[sizeof(float)];
        this.PositionY = new byte[sizeof(float)];
        this.Horizontal = new byte[sizeof(float)];
        this.Vertical = new byte[sizeof(float)];
        this.IsMoving = new byte[sizeof(bool)];
        this.IsAttacking = new byte[sizeof(bool)];
	}

    public GameObjectProperties(short id)
    {
       	this.PlayerID = BitConverter.GetBytes(id);
        this.PositionX = new byte[sizeof(float)];
        this.PositionY = new byte[sizeof(float)];
        this.Horizontal = new byte[sizeof(float)];
        this.Vertical = new byte[sizeof(float)];
        this.IsMoving = new byte[sizeof(bool)];
        this.IsAttacking = new byte[sizeof(bool)];
	}

    public void Update(float posX, float posY, float horizontal, float vertical, bool isMoving, bool isAttacking)
    {
      	this.PositionX = BitConverter.GetBytes(posX);
        this.PositionY = BitConverter.GetBytes(posY);
        this.Horizontal = BitConverter.GetBytes(horizontal);
        this.Vertical = BitConverter.GetBytes(vertical);
        this.IsMoving = BitConverter.GetBytes(isMoving);
        this.IsAttacking = BitConverter.GetBytes(isAttacking);
 	}

    public void UpdateID(short id)
    {
    	this.PlayerID = BitConverter.GetBytes(id);
    }

   	public void ToBuffer(ref byte[] buffer)
  	{
    	System.Buffer.BlockCopy(PlayerID, 0, buffer, 0, PlayerID.Length);
        System.Buffer.BlockCopy(PositionX, 0, buffer, PlayerID.Length, PositionX.Length);
        System.Buffer.BlockCopy(PositionY, 0, buffer, PlayerID.Length + PositionX.Length, PositionY.Length);
        System.Buffer.BlockCopy(Horizontal, 0, buffer, PlayerID.Length + PositionX.Length + PositionY.Length, Horizontal.Length);
        System.Buffer.BlockCopy(Vertical, 0, buffer, PlayerID.Length + PositionX.Length + PositionY.Length + Horizontal.Length, Vertical.Length);
        System.Buffer.BlockCopy(IsMoving, 0, buffer, PlayerID.Length + PositionX.Length + PositionY.Length + Horizontal.Length + Vertical.Length, IsMoving.Length);
        System.Buffer.BlockCopy(IsAttacking, 0, buffer, PlayerID.Length + PositionX.Length + PositionY.Length + Horizontal.Length + Vertical.Length + IsMoving.Length, IsAttacking.Length);
  	}

    public void FromBuffer(byte[] buffer)
    {
     	System.Buffer.BlockCopy(buffer, 0, PlayerID, 0, sizeof(short));
        System.Buffer.BlockCopy(buffer, sizeof(short), PositionX, 0, sizeof(float));
        System.Buffer.BlockCopy(buffer, sizeof(short) + sizeof(float), PositionY, 0, sizeof(float));
       	System.Buffer.BlockCopy(buffer, sizeof(short) + sizeof(float) + sizeof(float), Horizontal, 0, sizeof(float));
        System.Buffer.BlockCopy(buffer, sizeof(short) + sizeof(float) + sizeof(float) + sizeof(float), Vertical, 0, sizeof(float));
        System.Buffer.BlockCopy(buffer, sizeof(short) + sizeof(float) + sizeof(float) + sizeof(float) + sizeof(float), IsMoving, 0, sizeof(bool));
        System.Buffer.BlockCopy(buffer, sizeof(short) + sizeof(float) + sizeof(float) + sizeof(float) + sizeof(float) + sizeof(bool), IsAttacking, 0, sizeof(bool));
  	}

    public void FromBuffer(byte[] buffer, int offSet)
	{
    	System.Buffer.BlockCopy(buffer, offSet, PlayerID, 0, sizeof(short));
        System.Buffer.BlockCopy(buffer, offSet + sizeof(short), PositionX, 0, sizeof(float));
        System.Buffer.BlockCopy(buffer, offSet + sizeof(short) + sizeof(float), PositionY, 0, sizeof(float));
        System.Buffer.BlockCopy(buffer, sizeof(short) + sizeof(float) + sizeof(float), Horizontal, 0, sizeof(float));
        System.Buffer.BlockCopy(buffer, sizeof(short) + sizeof(float) + sizeof(float) + sizeof(float), Vertical, 0, sizeof(float));
        System.Buffer.BlockCopy(buffer, sizeof(short) + sizeof(float) + sizeof(float) + sizeof(float) + sizeof(float), IsMoving, 0, sizeof(bool));
        System.Buffer.BlockCopy(buffer, sizeof(short) + sizeof(float) + sizeof(float) + sizeof(float) + sizeof(float) + sizeof(bool), IsAttacking, 0, sizeof(bool));
   	}

    public void Convert(ref float posX, ref float posY, ref float horizontal, ref float vertical, ref bool isMoving, ref bool isAttacking)
	{
		posX = BitConverter.ToSingle (this.PositionX, 0);
		posY = BitConverter.ToSingle (this.PositionY, 0);
		horizontal = BitConverter.ToSingle (this.Horizontal, 0);
		vertical = BitConverter.ToSingle (this.Vertical, 0);
		isMoving = BitConverter.ToBoolean (this.IsMoving, 0);
		isAttacking = BitConverter.ToBoolean (this.IsAttacking, 0);
	}
}


